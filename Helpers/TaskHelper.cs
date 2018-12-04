using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers
{
    public static class TaskHelper
    {
        public static async Task<T> WithCancellation1<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(() => tcs.TrySetResult(true)))
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);
            return await task;
        }

        //该函数的作用是隐藏真正的异常信息，异常信息的记录可以在action方法中进行
        //然后再抛出一个简单的异常信息，比如果“保存失败，联系管理员”
        //program类中会统一处理这些异常，通过弹框方式显示给用户看
        public static ExcetpionTaskBuilder Catch(this Task task, Action<Exception> catchAction)
        {
            return new ExcetpionTaskBuilder(task, catchAction);
        }

        public static ExcetpionTaskBuilder<T> Catch<T>(this Task<T> task, Func<Exception, Exception> catchfunc)
        {
            return new ExcetpionTaskBuilder<T>(task, catchfunc);
        }

        public static ExcetpionTaskReturnBuilder<T> CatchReturn<T>(this Task<T> task, Func<Exception, T> catchfunc)
        {
            return new ExcetpionTaskReturnBuilder<T>(task, catchfunc);
        }
    }

    /// <summary>
    /// 捕获异常，该异步没有返回值
    /// 所以捕获到异常后是继续向上抛出还是处理异常，由回调函数决定
    /// </summary>
    public class ExcetpionTaskBuilder
    {
        private Action<Exception> CatchAction;
        private Task CatchTask;
        public ExcetpionTaskBuilder(Task task, Action<Exception> catchAction)
        {
            this.CatchTask = task;
            this.CatchAction = catchAction;
        }
        public async Task Finally(Action finallyAction = null)
        {
            try
            {
                await CatchTask;
            }
            catch (Exception ex)
            {
                CatchAction(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }
    }

    /// <summary>
    /// 捕获异常，会重新抛出异常
    /// 常用场景：进度条。
    /// 某个异步操作前启动进度条，如果异步操作异常，需要先隐藏进度条，再向上抛出异常
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcetpionTaskBuilder<T>
    {
        private Func<Exception, Exception> CatchFunc;
        private Task<T> CatchTask;
        public ExcetpionTaskBuilder(Task<T> task, Func<Exception, Exception> catchFunc)
        {
            this.CatchTask = task;
            this.CatchFunc = catchFunc;
        }
        public async Task<T> Finally(Action finallyAction = null)
        {
            try
            {
                return await CatchTask;
            }
            catch (Exception ex)
            {
                throw CatchFunc(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

    }
    /// <summary>
    /// 捕获异常，并返回一个默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcetpionTaskReturnBuilder<T>
    {
        private Func<Exception, T> CatchFunc;
        private Task<T> CatchTask;
        public ExcetpionTaskReturnBuilder(Task<T> task, Func<Exception, T> catchFunc)
        {
            this.CatchTask = task;
            this.CatchFunc = catchFunc;
        }
        public async Task<T> Finally(Action finallyAction = null)
        {
            try
            {
                return await CatchTask;
            }
            catch (Exception ex)
            {
                return CatchFunc(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }
    }
}

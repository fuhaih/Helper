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
    }

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
}

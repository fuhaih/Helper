namespace FHLog
{
    /**备注：
     * LogType枚举的值跟ConsoleColor的值相对应
     */

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 消息
        /// </summary>
        Info=7,
        /// <summary>
        /// 警告
        /// </summary>
        Warn=14,
        /// <summary>
        /// 异常
        /// </summary>
        Error=12,
        /// <summary>
        /// 错误
        /// </summary>
        Fatal=11
    }
}

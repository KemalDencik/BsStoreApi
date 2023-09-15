using NLog;
using Services.Contracts;
//LoggerManager sınıfı, hataları ve diğer günlük mesajlarını uygulama veya yazılımın çalışması sırasında izlemek, analiz etmek veya ayıklamak
//için kullanılır.Hata ayıklama veya sorun giderme sırasında günlük mesajları kullanıcıya veya geliştiriciye
//daha fazla bilgi sağlamak için de kullanılabilir.

namespace Services
{
    public class LoggerManager : ILoggerService
    {
        //logger kullanılabilir LogManager.GetCurrentClassLogger() metoduyla mevcut sınıfın logger'ını elde eder
        //sınıfın sunduğu çeşitli metotlar, ilgili logger üzerinden çağrılarak günlük mesajları belirli bir seviyede kaydedilir.
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        //logger üzerinden bir şey olursa debug a message yazdır
        public void LogDebug(string message) => logger.Debug(message);
        public void LogError(string message) => logger.Error(message);
        public void LogInfo(string message) => logger.Error(message);
        public void LogWarning(string message) => logger.Warn(message);
    }
}

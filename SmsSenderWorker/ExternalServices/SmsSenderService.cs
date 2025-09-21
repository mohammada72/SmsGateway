namespace SmsSenderWorker.ExternalServices;
public static class SmsSenderService
{
    public static async Task<bool> SendSms(string message, string phoneNumber)
    {
        await Task.Delay(100);
        var rand = new Random().Next(1,9);
        if (rand == 3)
        {
            Console.WriteLine($"Sending message to {phoneNumber} failed");
            return false;
        }
        Console.WriteLine($"Sending message to {phoneNumber} succeed");
        return true;
    }
}

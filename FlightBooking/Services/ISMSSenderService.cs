﻿namespace FlightBooking.Services
{
    public interface ISMSSenderService
    {
        Task SendSMSAsync(string toPhone, string message);

    }
}

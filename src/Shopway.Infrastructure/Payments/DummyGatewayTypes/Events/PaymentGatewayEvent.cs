﻿using Shopway.Infrastructure.Payments.DummyGatewayTypes.Sessions;

namespace Shopway.Infrastructure.Payments.DummyGatewayTypes.Events;

public class PaymentGatewayEvent
{
    public Data Data { get; set; } = null!;
    public string Type { get; set; } = string.Empty;
}

public class Data
{
    public Session Object { get; set; } = null!;
}

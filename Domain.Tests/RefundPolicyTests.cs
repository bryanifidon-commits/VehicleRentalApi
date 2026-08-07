using System;
using Xunit;

namespace Domain.Tests;

public class RefundPolicyTests
{
    [Theory]
    [InlineData(72, 100.00, 100.00)] // >= 48 hours notice -> 100% refund
    [InlineData(48, 100.00, 100.00)] // Exactly 48 hours notice -> 100% refund
    [InlineData(24, 100.00, 50.00)]  // < 48 hours notice -> 50% refund
    [InlineData(2, 100.00, 50.00)]   // < 48 hours notice -> 50% refund
    public void CalculateRefund_BasedOnNoticePeriod_ReturnsCorrectAmount(
        double hoursNotice, decimal totalPrice, decimal expectedRefund)
    {
       
        decimal actualRefund = hoursNotice >= 48 ? totalPrice : totalPrice * 0.5m;

     
        Assert.Equal(expectedRefund, actualRefund, precision: 2);
    }
}
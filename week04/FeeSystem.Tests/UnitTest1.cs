using NUnit.Framework;
using FeeSystem;
using System.Collections.Generic;

[TestFixture]
public class FeeCalculatorTests
{
    [Test]
    public void OutstandingBalance_NoPayments_ReturnsFullFee()
    {
        var calc = new FeeCalculator();
        var payments = new List<decimal>();

        var result = calc.OutstandingBalance(600m, payments);

        Assert.That(result, Is.EqualTo(600m));
    }

    [Test]
    public void OutstandingBalance_OnePartialPayment_ReturnsRemainingBalance()
    {
        var calc = new FeeCalculator();
        var payments = new List<decimal> { 200m };

        var result = calc.OutstandingBalance(600m, payments);

        Assert.That(result, Is.EqualTo(400m));
    }

    [Test]
    public void OutstandingBalance_SeveralInstallments_ReturnsCorrectBalance()
    {
        var calc = new FeeCalculator();
        var payments = new List<decimal> { 200m, 200m, 100m };

        var result = calc.OutstandingBalance(600m, payments);

        Assert.That(result, Is.EqualTo(100m));
    }

    [Test]
    public void OutstandingBalance_FullyPaid_ReturnsZero()
    {
        var calc = new FeeCalculator();
        var payments = new List<decimal> { 600m };

        var result = calc.OutstandingBalance(600m, payments);

        Assert.That(result, Is.EqualTo(0m));
    }

    [Test]
    public void OutstandingBalance_Overpayment_ReturnsNegativeBalance()
    {
        var calc = new FeeCalculator();
        var payments = new List<decimal> { 700m };

        var result = calc.OutstandingBalance(600m, payments);

        Assert.That(result, Is.EqualTo(-100m));
    }

    [Test]
    public void OutstandingBalance_NegativeFee_ThrowsArgumentException()
    {
        var calc = new FeeCalculator();
        var payments = new List<decimal>();

        Assert.That(
            () => calc.OutstandingBalance(-1m, payments),
            Throws.ArgumentException);
    }

    [Test]
    public void IsClearedForExams_ExactlyHalfPaid_ReturnsTrue()
    {
        var calc = new FeeCalculator();
        var payments = new List<decimal> { 300m };

        var result = calc.IsClearedForExams(600m, payments);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsClearedForExams_OneToeaUnderHalf_ReturnsFalse()
    {
        var calc = new FeeCalculator();
        var payments = new List<decimal> { 299.99m };

        var result = calc.IsClearedForExams(600m, payments);

        Assert.That(result, Is.False);
    }
}
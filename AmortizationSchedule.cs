using System.Collections.Generic;
namespace UXI_Mortgage
{
    internal class AmortizationSchedule
    {

        public static List<AmortizationEntry> GenerateSchedule(double principal, double annualRate, int termYears)
        {
            List<AmortizationEntry> schedule = new List<AmortizationEntry>();
            double monthlyRate = annualRate / 12 / 100;
            int totalPayments = termYears * 12;
            double monthlyPayment = MortgageCalculator.CalculateMonthlyPayment(principal, annualRate, termYears);

            for (int i = 0; i < totalPayments; i++)
            {
                double interestPayment = principal * monthlyRate;
                double principalPayment = monthlyPayment - interestPayment;
                principal -= principalPayment;

                schedule.Add(new AmortizationEntry
                {
                    PaymentNumber = i + 1,
                    Payment = monthlyPayment,
                    PrincipalPayment = principalPayment,
                    InterestPayment = interestPayment,
                    RemainingBalance = principal
                });
            }

            return schedule;
        }
    }

    public class AmortizationEntry
    {
        public int PaymentNumber { get; set; }
        public double Payment { get; set; }
        public double PrincipalPayment { get; set; }
        public double InterestPayment { get; set; }
        public double RemainingBalance { get; set; }
    }

}


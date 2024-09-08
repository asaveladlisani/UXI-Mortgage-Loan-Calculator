using System;

namespace UXI_Mortgage
{
    internal class MortgageCalculator
    {
        public static double CalculateMonthlyPayment(double principal, double annualRate, int termYears)
        {
            double monthlyRate = annualRate / 12 / 100;
            int totalPayments = termYears * 12;

            double numerator = monthlyRate * Math.Pow(1 + monthlyRate, totalPayments);
            double denominator = Math.Pow(1 + monthlyRate, totalPayments) - 1;

            return principal * (numerator / denominator);
        }

        public static double CalculateTotalInterest(double principal, double annualRate, int termYears)
        {
            double monthlyPayment = CalculateMonthlyPayment(principal, annualRate, termYears);
            int totalPayments = termYears * 12;

            return (monthlyPayment * totalPayments) - principal;
        }

        public static double CalculateTotalAmountPaid(double principal, double annualRate, int termYears)
        {
            double totalInterest = CalculateTotalInterest(principal, annualRate, termYears);
            return principal + totalInterest;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAb14
{
    public static class Model
    {
        private static List<Agent> agents = new List<Agent>();
        private static Bank bank;
        private static ArrivalProcess arrivalProcess;
        private static double T = 0;
        public static double Time { get { return T; } }
        private static Agent activeAgent;
        private static int customersAmount;
        public static int CAmount { get { return customersAmount; } }
        private static double lambdaArrival = 0;
        public static double lArr { get { return lambdaArrival; } }
        private static double lambdaOperator = 0;
        public static double lOper { get { return lambdaOperator; } }
        private static int operatorsAmount = 0;
        public static int opsAmount { get { return operatorsAmount; } }

        public static void Run(double lambdaArr, double lambdaOper, int opsAmount)
        {
            lambdaArrival = lambdaArr;
            lambdaOperator = lambdaOper;
            operatorsAmount = opsAmount;
            bank = new Bank();
            arrivalProcess = new ArrivalProcess(bank);
            agents.Add(bank);
            agents.Add(arrivalProcess);
            T = 0;
        }
        public static bool Iter()
        {
            if (continueCondition(T, activeAgent))
            {
                double tMin = double.MaxValue;
                activeAgent = null;
                foreach (Agent agent in agents)
                {
                    double tAgent = agent.getNextEventTime();
                    if (tAgent < tMin)
                    {
                        tMin = tAgent;
                        activeAgent = agent;
                    }
                }
                T = tMin;
                if (activeAgent != null) activeAgent.processEvent();
                customersAmount++;
                return true;
            }
            return false;
        }
        public static bool continueCondition(double t, Agent activeAgent)
        {
            return (t < 100);
        }
        public static int queueSize()
        {
            return bank.getQueueSize();
        }
        public static int getBusyOperatorsSize()
        {
            return bank.getBusyOperatorsSize();
        }
    }
}

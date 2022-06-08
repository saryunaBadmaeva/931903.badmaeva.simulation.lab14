using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAb14
{
    public abstract class Agent
    {
        public virtual double getNextEventTime()
        {
            return double.MaxValue;
        }
        public virtual void processEvent()
        {
            
        }
    }
    public class ArrivalProcess : Agent
    {
        private Random rnd = new Random();
        private double nextArrivalTime = 0;
        private Bank bank;
        public ArrivalProcess(Bank bank)
        {
            this.bank = bank;
            nextArrivalTime = simulateInterrivalTime();
        }
        private double simulateInterrivalTime()
        {
            return -Math.Log(rnd.NextDouble()) / Model.lArr;
        }
        public override double getNextEventTime()
        {
            return nextArrivalTime;
        }
        public override void processEvent()
        {
            Customer customer = new Customer();
            bank.customerArrived(customer);
            nextArrivalTime += simulateInterrivalTime();
        }
    }
    public class Customer : Agent
    {

    }
    public class Bank : Agent
    {
        private Service service = new Service(Model.opsAmount);
        private MyQueue queue = new MyQueue();
        public void customerArrived(Customer customer)
        {
            if (service.hasFreeOperator()) service.acceptCustomer(customer);
            else queue.acceptCustomer(customer);
        }
        public override double getNextEventTime()
        {
            return service.getNextEventTime();
        }
        public override void processEvent()
        {
            service.processEvent();
            if (queue.hasCustomers())
            {
                Customer cus = queue.takeCustomer();
                service.acceptCustomer(cus);
            }
        }
        internal int getBusyOperatorsSize()
        {
            return service.getBusyOperatorsSize();
        }
        public int getQueueSize()
        {
            return queue.getQueueSize();
        }
    }
    public class MyQueue : Agent
    {
        private Queue<Customer> queue = new Queue<Customer>();
        public void acceptCustomer(Customer customer)
        {
            queue.Enqueue(customer);
        }
        public bool hasCustomers()
        {
            return (queue.Count > 0);
        }
        public Customer takeCustomer()
        {
            return queue.Dequeue();
        }
        public int getQueueSize()
        {
            return queue.Count();
        }
    }
    public class Service : Agent
    {
        private List<Operator> operators = new List<Operator>();
        private Operator activeOper = new Operator();
        public Service(int N)
        {
            for (int i = 0; i < N; i++)
            {
                operators.Add(new Operator());
            }
        }
        public void acceptCustomer(Customer customer)
        {
            Operator op = findFreeOperator();
            if (op != null) op.acceptCustomer(customer);
        }
        public bool hasFreeOperator()
        {
            Operator op = findFreeOperator();
            return (op != null);
        }
        private Operator findFreeOperator()
        {
            foreach (Operator op in operators)
                if (op.isFree()) return op;
            return null;
        }
        public override double getNextEventTime()
        {
            double tMin = double.MaxValue;
            activeOper = null;
            foreach (Operator op in operators)
            {
                double tA = op.getNextEventTime(); 
                if (tA < tMin)
                {
                    tMin = tA;
                    activeOper = op;
                }
            }
            return tMin;
        }
        public override void processEvent()
        {
            if (activeOper != null)
                activeOper.processEvent();
        }
        public int getBusyOperatorsSize()
        {
            int size = 0;
            foreach (Operator op in operators)
                if (!op.isFree())
                    size++;
            return size;
        }
    }
    public class Operator : Agent
    {
        private Customer customerInService = null;
        private double endOfSeviceTime = double.MaxValue;
        private Random rnd = new Random();
        public void acceptCustomer(Customer customer)
        {
            if (isFree())
            {
                customerInService = customer;
                endOfSeviceTime = Model.Time + simulateServiceTime();
            }
        }
        private double simulateServiceTime()
        {
            return -Math.Log(rnd.NextDouble()) / Model.lOper;
        }
        public bool isFree()
        {
            return (customerInService == null);
        }
        public override double getNextEventTime()
        {
            return endOfSeviceTime;
        }
        public override void processEvent()
        {
            customerInService = null;
            endOfSeviceTime = double.MaxValue;
        }
    }
}

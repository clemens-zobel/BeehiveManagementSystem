using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeehiveManagementSystem
{
    class Queen : Bee, INotifyPropertyChanged
    {
        private IWorker[] workers = Array.Empty<IWorker>();
        private float unassignedWorkers = 0f;
        private float eggs = 0f;

        private const float EGGS_PER_SHIFT = 0.45f;
        private const float HONEY_PER_UNASSIGNED_WORKER = 0.5f;

        public string StatusReport { get; private set; }

        public override float CostPerShift
        {
            get
            {
                return 2.15f;
            }
        }

        public Queen() : base("Queen")
        {
            unassignedWorkers = 3;
            AssignBee("Honey Manufacturer");
            AssignBee("Nectar Collector");
            AssignBee("Egg Care");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void UpdateStatusReport()
        {
            StatusReport = $"{HoneyVault.StatusReport} \n \n" +
                    $"Egg count: {eggs: 0.00} \n" +
                    $"Unassigned workers: {Math.Round(unassignedWorkers, 2, MidpointRounding.ToZero)} \n" +
                    $"{WorkerStatus("Nectar Collector")}\n" +
                    $"{WorkerStatus("Honey Manufacturer")}\n" +
                    $"{WorkerStatus("Egg Care")}\n" +
                    $"TOTAL WORKERS: {workers.Length}";

            OnPropertyChanged("StatusReport");
        }

        protected override void DoJob()
        {
            eggs += EGGS_PER_SHIFT;

            foreach (IWorker worker in workers)
            {
                worker.WorkTheNextShift();
            }

            HoneyVault.ConsumeHoney(HONEY_PER_UNASSIGNED_WORKER * unassignedWorkers);
            UpdateStatusReport();
        }

        private void AddWorker(IWorker worker)
        {
            if (unassignedWorkers >= 1)
            {
                unassignedWorkers--;
                Array.Resize(ref workers, workers.Length + 1);
                workers[^1] = worker;
            }
        }

        public void AssignBee(string job)
        {
            switch (job)
            {
                case "Honey Manufacturer":
                    AddWorker(new HoneyManufacturer());
                    break;

                case "Nectar Collector":
                    AddWorker(new NectarCollector());
                    break;

                case "Egg Care":
                    AddWorker(new EggCare(this));
                    break;
            }
            UpdateStatusReport();
        }

        public void CareForEggs(float eggsToConvert)
        {
            if (eggs >= eggsToConvert)
            {
                eggs -= eggsToConvert;
                unassignedWorkers += eggsToConvert;
            }
        }

        private string WorkerStatus(string job)
        {
            int count = 0;
            foreach (IWorker worker in workers)
            {
                if (worker.Job == job)
                {
                    count++;
                }
            }

            string s = "s";
            if (count == 1)
            {
                s = "";
            }

            return $"{count} {job} bee{s}";
        }
    }
}

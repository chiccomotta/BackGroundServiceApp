using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BackGroundServiceApp
{
    public class BackGroundQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim semaphore = new SemaphoreSlim(0); // a nessun thread è permesso entrare (0)

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            // WaitAsync decrementa il sempahore count; poichè all'inizio è a zero il semaforo 
            // non permette a nessun thread di entrare
            Debug.WriteLine($"remaining threads: {semaphore.CurrentCount}");
            await semaphore.WaitAsync(cancellationToken);
            workItems.TryDequeue(out var workItem);

            return workItem;
        }

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            workItems.Enqueue(workItem);
            semaphore.Release();
        }
    }
}

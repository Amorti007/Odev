using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
class Program
{
    static ConcurrentQueue<string> wordQueue = new ConcurrentQueue<string>();
    static bool dosyaOkumaTamamlandiMi = false;

    static void Main(string[] args)
    {
        Console.Write("Thread sayısını girin: ");
        int threadSayisi = int.Parse(Console.ReadLine());
        Console.WriteLine();

        Thread okuyanThread = new Thread(DosyaOkuyanThread);
        okuyanThread.Start();

        Thread.Sleep(1000);

        Thread[] isThread = new Thread[threadSayisi];
        for (int i = 0; i < threadSayisi; i++)
        {
            isThread[i] = new Thread(CalisanThread);
            isThread[i].Start(i + 1);
        }

        okuyanThread.Join();
        foreach (var thread in isThread)
        {
            thread.Join();
        }

        Console.WriteLine("Bütün işlemler tamamlandı. Program sonlandı.");
    }

    static void DosyaOkuyanThread()
    {
        Console.WriteLine("Metin Dosyası Okunuyor.");
        string dosyaYolu = "C:\\Users\\Trindal\\Desktop\\Proje\\Odev\\Thread\\ThreadOkumaUygulamasi\\ThreadOkumaUygulamasi\\metin.txt";
        string metin = File.ReadAllText(dosyaYolu);
        string[] kelimeler = metin.Split(' ');
        Console.WriteLine();

        foreach (var kelime in kelimeler)
        {
            wordQueue.Enqueue(kelime);
            Console.WriteLine("Kuyruğa eklendi: " + kelime);
        }

        dosyaOkumaTamamlandiMi = true;

        Console.WriteLine();
        Console.WriteLine("Dosyadan okuma tamamlandı.");
        Console.WriteLine();
    }

    static void CalisanThread(object threadNumarasi)
    {
        int threadIndex = (int)threadNumarasi;

        while (true)
        {
            string sozcuk;
            if (wordQueue.TryDequeue(out sozcuk))
            {
                Console.WriteLine("Thread " + threadIndex + " - " + sozcuk + " : " + sozcuk.Length);
                Thread.Sleep(3000);
            }
            else
            {
                if (dosyaOkumaTamamlandiMi)
                    break;

                Console.WriteLine("Thread " + threadIndex + " - Kuyruk boş. Bekleniyor...");
                Thread.Sleep(3000);
            }
        }
    }
}

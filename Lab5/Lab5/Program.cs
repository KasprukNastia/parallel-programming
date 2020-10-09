using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            // First collection
            Task<List<int>> onlyGreaterThanAverageFirstOrderedCollection = CreateRandomCollection(collectionId: 1)
                .ContinueWith(collection =>
                {
                    var average = collection.Result.Average();
                    Console.WriteLine($"The average of first collection = {average}\n");

                    var filtered = collection.Result.Where(e => e > average);
                    Console.WriteLine(
                        $"Filtered first collection (only greater than average): {string.Join(", ", filtered)}\n");

                    return filtered;
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(collection => 
                {
                    var ordered = collection.Result.OrderBy(e => e).ToList();
                    Console.WriteLine(
                       $"Ordered first collection after filtering: {string.Join(", ", ordered)}\n");

                    return ordered;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            // Second collection
            Task<List<int>> onlyLessThanAverageSecondOrderedCollection = CreateRandomCollection(collectionId: 2)
                .ContinueWith(collection =>
                {
                    var average = collection.Result.Average();
                    Console.WriteLine($"The average of second collection = {average}\n");

                    var filtered = collection.Result.Where(e => e < average);
                    Console.WriteLine(
                        $"Filtered second collection (only less than average): {string.Join(", ", filtered)}\n");

                    return filtered;
                })
                .ContinueWith(collection =>
                {
                    var ordered = collection.Result.OrderBy(e => e).ToList();
                    Console.WriteLine(
                       $"Ordered second collection after filtering: {string.Join(", ", ordered)}\n");

                    return ordered;
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

            // Merged
            Task<List<int>> finalResult = Task.WhenAll(
                onlyGreaterThanAverageFirstOrderedCollection,
                onlyLessThanAverageSecondOrderedCollection)
                .ContinueWith(collections => 
                {
                    var mergedOrderedCollection = collections.Result.SelectMany(e => e).OrderBy(e => e).ToList();
                    Console.WriteLine($"Finally merged and ordered collection: {string.Join(", ", mergedOrderedCollection)}\n");

                    return mergedOrderedCollection;
                },TaskContinuationOptions.OnlyOnRanToCompletion);

            Task.WaitAll(finalResult);
        }

        public static Task<List<int>> CreateRandomCollection(int collectionId)
        {
            var random = new Random();

            var collectionSize = random.Next(5, 10);
            var collection = new List<int>(collectionSize);
            for (int i = 0; i < collectionSize; i++)
                collection.Add(random.Next(0, 100));

            Console.WriteLine(
                $"Generated collection with id {collectionId} of size {collectionSize}: {string.Join(", ", collection)}\n");

            return Task.FromResult(collection);
        }
    }
}

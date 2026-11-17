using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MailSystem.Server.Services
{
    public class SpamClassifier
    {
        private readonly Dictionary<string, int> _hamWordCounts = new();
        private readonly Dictionary<string, int> _spamWordCounts = new();
        private int _hamDocCount = 0;
        private int _spamDocCount = 0;
        private int _hamTotalTokens = 0;
        private int _spamTotalTokens = 0;
        private HashSet<string> _vocabulary = new();

        private static readonly HashSet<string> StopWords = new()
        {
            "the", "and", "to", "of", "in", "i", "is", "that", "it", "on", "you", "this", "for", "but", "with", "are", "have", "be", "at", "or", "as", "was", "so", "if", "out", "not", "a", "an"
        };

        public void Train(string label, string text)
        {
            var tokens = Tokenize(text);
            if (tokens.Count == 0) return;

            if (label.Equals("spam", StringComparison.OrdinalIgnoreCase))
            {
                _spamDocCount++;
                _spamTotalTokens += tokens.Count;
                foreach (var token in tokens)
                {
                    _spamWordCounts[token] = _spamWordCounts.GetValueOrDefault(token) + 1;
                    _vocabulary.Add(token);
                }
            }
            else // ham
            {
                _hamDocCount++;
                _hamTotalTokens += tokens.Count;
                foreach (var token in tokens)
                {
                    _hamWordCounts[token] = _hamWordCounts.GetValueOrDefault(token) + 1;
                    _vocabulary.Add(token);
                }
            }
        }

        public (double SpamProbability, List<string> TopTokens) Classify(string text)
        {
            var tokens = Tokenize(text);
            if (tokens.Count == 0) return (0.0, new List<string>());

            double spamScore = Math.Log((double)_spamDocCount / (_spamDocCount + _hamDocCount)); // Prior
            double hamScore = Math.Log((double)_hamDocCount / (_spamDocCount + _hamDocCount));   // Prior

            int vocabSize = _vocabulary.Count;

            // Dictionary to track which words contributed the most to the score
            var wordContributions = new Dictionary<string, double>();

            foreach (var token in tokens)
            {
                // Laplace smoothing: (count + 1) / (total + vocabSize)
                int spamCount = _spamWordCounts.GetValueOrDefault(token);
                int hamCount = _hamWordCounts.GetValueOrDefault(token);

                double spamProb = (double)(spamCount + 1) / (_spamTotalTokens + vocabSize);
                double hamProb = (double)(hamCount + 1) / (_hamTotalTokens + vocabSize);

                spamScore += Math.Log(spamProb);
                hamScore += Math.Log(hamProb);

                // Track the difference (higher means more spammy)
                wordContributions[token] = Math.Log(spamProb) - Math.Log(hamProb);
            }

            // Sigmoid function to convert the difference into a 0.0 - 1.0 probability
            double difference = spamScore - hamScore;
            double probability = 1.0 / (1.0 + Math.Exp(-difference));

            // Get the top 5 words that contributed most to the spam score
            var topTokens = wordContributions
                .OrderByDescending(kv => kv.Value)
                .Take(5)
                .Select(kv => kv.Key)
                .ToList();

            return (probability, topTokens);
        }

        private List<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();

            // Lowercase, split on non-letters, drop stop-words and short tokens
            return Regex.Split(text.ToLower(), @"[^a-z]+")
                .Where(t => t.Length >= 3 && !StopWords.Contains(t))
                .ToList();
        }
        public int TotalHamDocs => _hamDocCount;
        public int TotalSpamDocs => _spamDocCount;

        public void Reset()
        {
            _hamWordCounts.Clear();
            _spamWordCounts.Clear();
            _vocabulary.Clear();
            _hamDocCount = 0;
            _spamDocCount = 0;
            _hamTotalTokens = 0;
            _spamTotalTokens = 0;
        }
    }
}
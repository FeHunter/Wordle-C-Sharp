using System;
using System.Text.Json;
using RestSharp;

namespace WordleGame
{
    class Program
    {
        private static string[] words;
        private static string _tryWord;
        private static char[] _currentTry;
        private static string _wordToFind;
        private static bool end = false;
        static Random random = new Random();

        static void Main ()
        {
            // First message
            Console.Clear();
            Console.WriteLine("Starting game, loading words...");

            words = LoadWords();

            // Palavra aleatória
            _wordToFind = words[random.Next(0, words.Length-1)];

            // Colocar a tamanho da palavra para a char array
            _currentTry = new string('_', _wordToFind.Length).ToCharArray();
            
            // Loop game
            do
            {
                Start();
                if (end){
                    EndGame();
                }
            }while (!end);
        }

        static void Start ()
        {
            Console.Clear();
            Console.WriteLine("\n\nGuess the word! The Game\n\n");
            Console.WriteLine($"Guess this word: {new string(_currentTry)} \nType 'help' for a tip\nType 'exit' to quit");
            Console.Write("\nType: ");
            _tryWord = Console.ReadLine();

            // Show help
            if (_tryWord == "help")
            {
                Console.WriteLine($"Word to find: {new string(_wordToFind.Reverse().ToArray())}"); 
                Console.Read();
            }
            if (_tryWord == "exit")
            {
                end = true;
            }

            CheckAnswer();
        }

        static void EndGame()
        {
            Console.Clear();
            Console.WriteLine($"\n\nCongratulations, you Win!!!\n\nThe word was *{_wordToFind}* \n\n");
            string playAgain;
            Console.WriteLine("Want to play again? \n(Y) for yes (N) for no");
            playAgain = Console.ReadLine();
            if (playAgain == "y"){
                Console.Clear();
                Console.WriteLine("Loading...");
                // Reset word
                _wordToFind = words[random.Next(0, words.Length-1)];
                _currentTry = new string('_', _wordToFind.Length).ToCharArray();

                end = false;
                playAgain = "";
            }
        }

        static void CheckAnswer ()
        {
            for (int i = 0; i < _wordToFind.Length; i++)
            {
                if (i < _tryWord.Length && _tryWord[i] == _wordToFind[i])
                {
                    _currentTry[i] = _tryWord[i];
                }
            }
            if (new string(_currentTry) == _tryWord)
            {
                end = true;
            }
        }

        static string[] LoadWords ()
        {
            string _url = "https://random-word-api.herokuapp.com/all";
            var _client = new RestClient(_url);
            var _request = new RestRequest();
            var _response = _client.Get(_request);

            if (_response.IsSuccessful)
            {
                try
                {
                    // Tratamento da resposta Json para string
                    string[] loadedWords = JsonSerializer.Deserialize<string[]>(_response.Content);
                    return loadedWords;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erro: " + e.Message);
                    throw new Exception ("Erro" + e.Message);
                }
            }
            else
            {
                throw new Exception ("Erro to load API");
            }
        }
    }
}
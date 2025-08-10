using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HoroscopoApp{
    class Program{
        static async Task Main(string[] args){
            Console.Clear();
            Console.WriteLine(" - - - - - - - - - - Consulta de Horoscopo C# - - - - - - - - - -");

            // ---------- BLOCO NOME ----------
            Console.Write("\nDigite seu nome: ");
            string? nome = Console.ReadLine();
            while (string.IsNullOrEmpty(nome))
            {
                Console.WriteLine("\nO nome não pode ficar em branco. Digite novamente.");
                Console.Write("Digite seu nome: ");
                nome = Console.ReadLine();
            }

            DateTime dataNascimento;
            Console.Write("Digite sua data de nascimento (Ex: 18/09/2001): ");
            string? inputNascimento = Console.ReadLine();

            while (!DateTime.TryParseExact(inputNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNascimento))
            {
                Console.WriteLine("\nData inválida ou em formato incorreto. Tente novamente.");
                Console.Write("Digite sua data de nascimento (Ex: 18/09/2001): ");
                inputNascimento = Console.ReadLine();
            }

            var signo = Verificar_Signo(dataNascimento);
            Console.WriteLine($"\nOlá {nome}! Seu signo é {signo["pt"]}. Buscando seu horóscopo do dia...");

            await BuscarHoroscopo(signo["en"]);

            Console.WriteLine("\nPressione Enter para sair...");
            Console.ReadLine();
        }

        public static async Task BuscarHoroscopo(string signo)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url_horoscopo = $"https://horoscope-app-api.vercel.app/api/v1/get-horoscope/monthly?sign={signo}";
                    string response_horoscopo = await client.GetStringAsync(url_horoscopo);
                    string horoscopoEN;


                    using (JsonDocument doc = JsonDocument.Parse(response_horoscopo))
                    {
                        JsonElement root = doc.RootElement;
                        horoscopoEN = root.GetProperty("data").GetProperty("horoscope_data").GetString();
                    }

                    string horoscopo_uri = Uri.EscapeDataString(horoscopoEN ?? "");
                    string url_traducao = $"https://api.mymemory.translated.net/get?q={horoscopo_uri}&langpair=en|pt&de=tiago.lucas.oliveira18@gmail.com";
                    string response_traducao = await client.GetStringAsync(url_traducao);
                    using (JsonDocument doc = JsonDocument.Parse(response_traducao))
                    {
                        JsonElement root = doc.RootElement;
                        string? horoscopoPT = root.GetProperty("responseData").GetProperty("translatedText").GetString();
                        //horoscopoPT = horoscopoPT.Replace(". ", ".\n");
                        Console.WriteLine("\n Horóscopo");
                        Console.WriteLine(horoscopoPT);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"\nErro ao conectar com a API: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\nOcorreu um erro inesperado: {e.Message}");
                }
            }
        }

        public static Dictionary<string, string> Verificar_Signo(DateTime dataNascimento)
        {
            int dia = dataNascimento.Day;
            int mes = dataNascimento.Month;

            if ((mes == 3 && dia >= 21) || (mes == 4 && dia <= 20))
                return new Dictionary<string, string> { { "en", "Aries" }, { "pt", "Áries" } };
            if ((mes == 4 && dia >= 21) || (mes == 5 && dia <= 20))
                return new Dictionary<string, string> { { "en", "Taurus" }, { "pt", "Touro" } };
            if ((mes == 5 && dia >= 21) || (mes == 6 && dia <= 20))
                return new Dictionary<string, string> { { "en", "Gemini" }, { "pt", "Gêmeos" } };
            if ((mes == 6 && dia >= 21) || (mes == 7 && dia <= 22))
                return new Dictionary<string, string> { { "en", "Cancer" }, { "pt", "Câncer" } };
            if ((mes == 7 && dia >= 23) || (mes == 8 && dia <= 22))
                return new Dictionary<string, string> { { "en", "Leo" }, { "pt", "Leão" } };
            if ((mes == 8 && dia >= 23) || (mes == 9 && dia <= 22))
                return new Dictionary<string, string> { { "en", "Virgo" }, { "pt", "Virgem" } };
            if ((mes == 9 && dia >= 23) || (mes == 10 && dia <= 22))
                return new Dictionary<string, string> { { "en", "Libra" }, { "pt", "Libra" } };
            if ((mes == 10 && dia >= 23) || (mes == 11 && dia <= 21))
                return new Dictionary<string, string> { { "en", "Scorpio" }, { "pt", "Escorpião" } };
            if ((mes == 11 && dia >= 22) || (mes == 12 && dia <= 21))
                return new Dictionary<string, string> { { "en", "Sagittarius" }, { "pt", "Sargitário" } };
            if ((mes == 12 && dia >= 22) || (mes == 1 && dia <= 20))
                return new Dictionary<string, string> { { "en", "Capricorn" }, { "pt", "Capricórnio" } };
            if ((mes == 1 && dia >= 21) || (mes == 2 && dia <= 18))
                return new Dictionary<string, string> { { "en", "Aquarius" }, { "pt", "Aquário" } };
            if ((mes == 2 && dia >= 19) || (mes == 3 && dia <= 20))
                return new Dictionary<string, string> { { "en", "Pisces" }, { "pt", "Peixes" } };

            return new Dictionary<string, string> { { "en", "N/A" }, { "pt", "N/A" } };
        }
    }
}
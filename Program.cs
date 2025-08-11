using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// Adicionar data
namespace HoroscopoApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine(" - - - - - - - - - - Consulta de Horoscopo C# - - - - - - - - - -");

            string nome = info_nome();
            DateTime dataNascimento = info_nasc();

            var data = info_data();
            var signo = Verificar_Signo(dataNascimento);

            Console.WriteLine($"\nOlá {nome}! Aguarde, enquanto buscamos o seu horóscopo...\n\nSigno: {signo["pt"]}\nData: {data["dia"]}");

            await BuscarHoroscopo(signo["en"]);

            Console.WriteLine("\nPressione Enter para finalizar o programa...");
            Console.ReadLine();
        }

        // Input Nome
        public static string info_nome()
        {
            Console.Write("\nDigite seu nome: ");
            string? nome = Console.ReadLine();

            while (string.IsNullOrEmpty(nome))
            {
                Console.WriteLine("\nO nome não pode ficar em branco. Digite novamente.");
                Console.Write("Digite seu nome: ");
                nome = Console.ReadLine();
            }

            return nome!;
        }

        // Input Nascimento
        public static DateTime info_nasc()
        {
            DateTime dataNascimento;
            Console.Write("Digite sua data de nascimento (Ex: 18/09/2001): ");
            string? inputNascimento = Console.ReadLine();

            while (!DateTime.TryParseExact(inputNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNascimento))
            {
                Console.WriteLine("\nData inválida ou em formato incorreto. Tente novamente.");
                Console.Write("Digite sua data de nascimento (Ex: 18/09/2001): ");
                inputNascimento = Console.ReadLine();
            }

            return dataNascimento!;
        }

        // Formatação de Datas
        public static Dictionary<string, string> info_data()
        {
            string data_01 = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("pt-BR"));
            string data_02 = DateTime.Now.ToString("dd'/'MMMM", new System.Globalization.CultureInfo("pt-BR"));
            string data_03 = DateTime.Now.ToString("MMMM'/'yyyy", new System.Globalization.CultureInfo("pt-BR"));

            return new Dictionary<string, string>{
                {"escrito", data_01},
                {"dia", data_02},
                {"mes", data_03}
            };
        }

        public static async Task BuscarHoroscopo(string signo){
            using (HttpClient client = new HttpClient()){
                try
                {
                    string url_horoscopo_dia = $"https://horoscope-app-api.vercel.app/api/v1/get-horoscope/daily?sign={signo}&day=TODAY";
                    string response_horoscopo_dia = await client.GetStringAsync(url_horoscopo_dia);

                    string url_horoscopo_mes = $"https://horoscope-app-api.vercel.app/api/v1/get-horoscope/monthly?sign={signo}";
                    string response_horoscopo_mes = await client.GetStringAsync(url_horoscopo_mes);

                    string horoscopoEN_dia;
                    string horoscopoEN_mes;

                    var data = info_data();

                    using (JsonDocument doc = JsonDocument.Parse(response_horoscopo_dia))
                    {
                        JsonElement root = doc.RootElement;
                        horoscopoEN_dia = root.GetProperty("data").GetProperty("horoscope_data").GetString() ?? "";
                    }
                    using (JsonDocument doc = JsonDocument.Parse(response_horoscopo_mes))
                    {
                        JsonElement root = doc.RootElement;
                        horoscopoEN_mes = root.GetProperty("data").GetProperty("horoscope_data").GetString() ?? "";
                    }

                    string horoscopo_uri_dia = Uri.EscapeDataString(horoscopoEN_dia ?? "");
                    //string horoscopo_uri_mes = Uri.EscapeDataString(horoscopoEN_mes ?? "");

                    string url_traducao_dia = $"https://api.mymemory.translated.net/get?q={horoscopo_uri_dia}&langpair=en|pt&de=tiago.lucas.oliveira18@gmail.com";
                    //string url_traducao_mes = $"https://api.mymemory.translated.net/get?q={horoscopo_uri_mes}&langpair=en|pt&de=tiago.lucas.oliveira18@gmail.com";

                    string response_dia = await client.GetStringAsync(url_traducao_dia);
                    //string response_mes = await client.GetStringAsync(url_traducao_mes);

                    using (JsonDocument doc = JsonDocument.Parse(response_dia))
                    {
                        JsonElement root = doc.RootElement;
                        string? horoscopoPT_dia = root.GetProperty("responseData").GetProperty("translatedText").GetString();
                        //horoscopoPT = horoscopoPT.Replace(". ", ".\n");
                        Console.WriteLine($"\n - - - - - - - - - - Horóscopo do Dia - {data["dia"]} - - - - - - - - - -\n");
                        Console.WriteLine(horoscopoPT_dia);
                    }

                    string? horoscopoPT_mes;
                    const int limiteApi = 450;

                    if (horoscopoEN_mes.Length <= limiteApi){
                        string horoscopo_uri_mes = Uri.EscapeDataString(horoscopoEN_mes);
                        string url_traducao_mes = $"https://api.mymemory.translated.net/get?q={horoscopo_uri_mes}&langpair=en|pt&de=tiago.lucas.oliveira18@gmail.com";
                        string response_mes = await client.GetStringAsync(url_traducao_mes);

                        using (JsonDocument doc = JsonDocument.Parse(response_mes)){
                            horoscopoPT_mes = doc.RootElement.GetProperty("responseData").GetProperty("translatedText").GetString();}}
                    else{
                        int meio = horoscopoEN_mes.Length / 2;
                        int quebra = horoscopoEN_mes.LastIndexOf(' ', meio);

                        string parte1 = horoscopoEN_mes.Substring(0, quebra);
                        string parte2 = horoscopoEN_mes.Substring(quebra);

                        string uri_parte1 = Uri.EscapeDataString(parte1);
                        string url_parte1 = $"https://api.mymemory.translated.net/get?q={uri_parte1}&langpair=en|pt&de=tiago.lucas.oliveira18@gmail.com";
                        string response_parte1 = await client.GetStringAsync(url_parte1);
                        string? traducao_parte1;
                        using (JsonDocument doc = JsonDocument.Parse(response_parte1)){
                            traducao_parte1 = doc.RootElement.GetProperty("responseData").GetProperty("translatedText").GetString();}

                        string uri_parte2 = Uri.EscapeDataString(parte2);
                        string url_parte2 = $"https://api.mymemory.translated.net/get?q={uri_parte2}&langpair=en|pt&de=tiago.lucas.oliveira18@gmail.com";
                        string response_parte2 = await client.GetStringAsync(url_parte2);
                        string? traducao_parte2;
                        using (JsonDocument doc = JsonDocument.Parse(response_parte2)){
                            traducao_parte2 = doc.RootElement.GetProperty("responseData").GetProperty("translatedText").GetString();}

                        horoscopoPT_mes = traducao_parte1 + traducao_parte2;

                    Console.WriteLine($"\n - - - - - - - - - - Horóscopo do Mês - {data["mes"]} - - - - - - - - - -\n");
                    Console.WriteLine(horoscopoPT_mes);
                    }          
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"\nErro ao conectar com a API: {e.Message}");
                }
                catch (Exception e) { Console.WriteLine($"\nErro: {e.Message}"); }
            }
        }

        // Validação do Signo
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SapConsultaCep.Servico.Modelo
{
    class ConsultaCnpj
    {

        private static string EnderecoUrl = "https://www.receitaws.com.br/v1/cnpj/{0}";

        ////https://www.receitaws.com.br/v1/cnpj/[cnpj
        ////http://viacep.com.br/ws/{0}/json/


        public static Endereco BuscarEnderecoViaCep(string cnpj)
        {
            string txtResults = string.Empty;
            string txtstatus = string.Empty;

            try
            {
                string NovoEnderecoURL = string.Format(EnderecoUrl, cnpj);

                WebClient wc = new WebClient();

                wc.DownloadString(NovoEnderecoURL);

                string Conteudo = wc.DownloadString(NovoEnderecoURL);
                Endereco end = JsonConvert.DeserializeObject<Endereco>(Conteudo);
                return end;

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse err = ex.Response as HttpWebResponse;

                    if (err != null)
                    {
                        string htmlResponse = new StreamReader(err.GetResponseStream()).ReadToEnd();
                        txtResults = string.Format("{0}", err.StatusDescription, htmlResponse);
                        txtstatus = string.Format("{0}", err.StatusCode, htmlResponse);

                    }
                }
                else
                {

                }

                Endereco end = new Endereco();
                end.message = txtResults.ToString();
                end.status = txtstatus.ToString();
                return end;
            }

        }


        public static Sintegra ConsultaSintegra(string token, string cnpj, string plugin)


        {

            using (HttpClient client = new HttpClient())
            {
                String url = "https://www.sintegraws.com.br/api/v1/execute-api.php?token=" + token + "&cnpj=" + cnpj + "&plugin=" + plugin;

                var response = client.GetAsync(url).Result;

                using (HttpContent content = response.Content)
                {
                    Task result = content.ReadAsStringAsync();

                    string jsonRetorno = response.Content.ReadAsStringAsync().Result;

                    Sintegra sintegra = JsonConvert.DeserializeObject<Sintegra>(jsonRetorno);
                    return sintegra;

                }

            }




        }


        public static SNacional SNacional(string token, string cnpj, string plugin)
        {
            using (HttpClient client = new HttpClient())
            {
                String url = "https://www.sintegraws.com.br/api/v1/execute-api.php?token=" + token + "&cnpj=" + cnpj + "&plugin=" + plugin;

                var response = client.GetAsync(url).Result;

                using (HttpContent content = response.Content)
                {
                    Task result = content.ReadAsStringAsync();

                    string jsonRetorno = response.Content.ReadAsStringAsync().Result;

                    SNacional snacional = JsonConvert.DeserializeObject<SNacional>(jsonRetorno);
                    return snacional;

                }

            }

        }
    }
}

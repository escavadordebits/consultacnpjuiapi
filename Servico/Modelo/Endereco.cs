using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SapConsultaCep.Servico.Modelo
{
    public class Endereco
    {

            public string cep { get; set; }
            public string logradouro { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string logradoro { get; set; }
        
            public string cnpj { get; set; }
          
            public string uf { get; set; }
            public string unidade { get; set; }
            public string email { get; set; }
            public string telefone { get; set; }
            public string nome { get; set; }
            public string municipio { get; set; }
            public string numero { get; set; }
            public string status { get; set; }
            public List<AtividadePrincipal> atividade_principal { get; set; }
            public string message { get; set; }
          



    }


    public class AtividadePrincipal
    {
        public string text { get; set; }
        public string code { get; set; }
    }


   

}

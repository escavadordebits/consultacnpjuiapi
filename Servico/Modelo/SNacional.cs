using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapConsultaCep.Servico.Modelo
{
        public class SNacional
        {
            public String code { get; set; }
            public String status { get; set; }
            public String message { get; set; }
            public String cnpj { get; set; }
            public String cnpj_matriz { get; set; }
            public String nome_empresarial { get; set; }
            public String situacao_simples_nacional { get; set; }
            public String situacao_simei { get; set; }
            public String situacao_simples_nacional_anterior { get; set; }
            public String situacao_simei_anterior { get; set; }
            public String agendamentos { get; set; }
            public String eventos_futuros_simples_nacional { get; set; }
            public String eventos_futuros_simples_simei { get; set; }
        }
}

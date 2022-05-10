using SapConsultaCep.Servico.Modelo;
using System;
using System.Windows.Forms;
using UI = SAPbouiCOM;
using SAPbobsCOM;


namespace SapConsultaCep
{
    public class Login 

    {

        private UI.Application SBO_Application;
        private UI.Form oForm;
        private static UI.Item oItem;
        private UI.Button oButton;
        private UI.EditText oEdittext;
        private UI.StaticText oStaticText;
        private UI.Column oColumm;
        private UI.ComboBox oComoBox;
        private UI.Matrix oMatrix;
        private string v_cnae;
        private string v_codpn;
        private string cnpj2;
        private string IE;
        private string Situacaocnpj;
        private string SituacaoIe; 

        private SAPbobsCOM.Company oCompany;

        UI.SboGuiApi sboGuiApi = null;



        public void ConectaSap()
        {
            sboGuiApi = new SAPbouiCOM.SboGuiApi();

            

            string sConnectionString = null;
            sConnectionString = System.Convert.ToString(Environment.GetCommandLineArgs().GetValue(1));
            sboGuiApi.Connect(sConnectionString);
            SBO_Application = sboGuiApi.GetApplication(-1);

            if (SBO_Application != null)
            {
                SBO_Application.StatusBar.SetText("Consulta CNPJ Conectado", UI.BoMessageTime.bmt_Short, UI.BoStatusBarMessageType.smt_Success);

            }
            else
            {
                SBO_Application.StatusBar.SetText("Erro ao Conectar UI", UI.BoMessageTime.bmt_Long, UI.BoStatusBarMessageType.smt_Error);
            }

            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);

            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ConsultaCEp);

            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ConsultaCnae);

        }

        private void SBO_Application_ConsultaCEp(string FormUID, ref UI.ItemEvent pVal, out bool BubbleEvent)
        {

            BubbleEvent = true;
          

            if ((pVal.FormType == 134) & (pVal.ItemUID == "ConsCNPJ") & (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) & (pVal.BeforeAction == true))

            {


                SBO_Application.StatusBar.SetText("Realizando a Consulta, Favor aguardar!!", UI.BoMessageTime.bmt_Short, UI.BoStatusBarMessageType.smt_Success);


                oForm = SBO_Application.Forms.GetForm("134", 1);
             

                oEdittext = oForm.Items.Item("TxtCnpj").Specific;

                cnpj2 = oEdittext.Value;

                string cnpj = cnpj2.OnlyNumbers();

                //string cnpj = "20919777000161";
                Endereco End = ConsultaCnpj.BuscarEnderecoViaCep(cnpj);

                string token = "9C7E2A16-D20B-4F8D-9833-14835BEBD2E6";

                //Sintegra Sintegra = ConsultaCnpj.ConsultaSintegra(token, cnpj, "ST");


                Sintegra Sintegra = ConsultaCnpj.ConsultaSintegra(token, cnpj, "ST");


                if (Sintegra.status == "OK")
                
                {
                    IE = Sintegra.inscricao_estadual;
                    Situacaocnpj = Sintegra.situacao_cnpj;
                    SituacaoIe = Sintegra.situacao_ie;


                }




                if ( End.status == "429")
                {
                    SBO_Application.StatusBar.SetText("Muitas Consultas, tentar novamente mais tarde", UI.BoMessageTime.bmt_Short, UI.BoStatusBarMessageType.smt_Error);

                }


                if ( End.status == "ERROR" ) 
                {
                    string message = string.Format("{0}", End.message);

                    SBO_Application.StatusBar.SetText(message, UI.BoMessageTime.bmt_Short, UI.BoStatusBarMessageType.smt_Error);

                    

                }else
                {


                    string cepresp2 = string.Format("{0}", End.cep);

                    string cepresp = cepresp2.OnlyNumbers();

                    string razao = string.Format("{0}", End.nome);
                    string bairro = string.Format("{0}", End.bairro);
                    string log;
                    string rua;
                    if (string.IsNullOrEmpty(End.logradouro))
                    {
                        log = "";
                        rua = "";
                    }

                    else
                    {

                        log = string.Format("{0}",End.logradouro).Substring(0,3);
                        int i = string.Format("{0}", End.logradouro).Length;

                        rua = string.Format("{0}", End.logradouro).Substring(2, i - 2);

                    }



                    string cidade = string.Format("{0}", End.municipio);
                    string numero = string.Format("{0}", End.numero);

                    string email = "";

                    if (End.email != null)
                    {

                        email = string.Format("{0}", End.email);


                    }else
                    {
                        email = "N/I";
                    }

                           
                    string telefone = "";

                    if (string.IsNullOrWhiteSpace(End.telefone))
                    {
                        
                        telefone = "";
                    }
                    else
                    {
                        telefone = string.Format("{0}", End.telefone);
                    }


                    string comp = string.Format("{0}", End.complemento);
                    string uf = string.Format("{0}", End.uf);

                    if ( End.atividade_principal[0].code != null)
                    {
                        string cnae1 = Convert.ToString(End.atividade_principal[0].code);
                        string cnae = cnae1.OnlyNumbers();
                        v_cnae = cnae.Substring(0, 4) + "-" + cnae.Substring(4, 1) + "/" + cnae.Substring(5, 2);

                    }





                    oEdittext = oForm.Items.Item("7").Specific;
                    oEdittext.Value = razao;





                    int panelvl = oForm.PaneLevel;

                    oForm.PaneLevel = 1;

                    oEdittext = oForm.Items.Item("60").Specific;
                    oEdittext.Value = email;

                    if (telefone != "")
                    {
                        oEdittext = oForm.Items.Item("43").Specific;
                        oEdittext.Value = telefone.Substring(0, 14);
                    }


                    oComoBox = oForm.Items.Item("40").Specific;
                    string v_tipo = oComoBox.Selected.Value;


                     
                   if (v_tipo == "C")
                    {


                        oEdittext = oForm.Items.Item("5").Specific;
                        oEdittext.Value = "C" + cnpj;

                    }

                    if (v_tipo == "S")
                    {


                        oEdittext = oForm.Items.Item("5").Specific;
                        oEdittext.Value = "F" + cnpj;

                    }


                    oEdittext = oForm.Items.Item("5").Specific;
                    v_codpn = oEdittext.Value;



                    oForm.PaneLevel = 7;


                    oMatrix = oForm.Items.Item("178").Specific;
                    oColumm = oMatrix.Columns.Item("1");
                    oEdittext = oColumm.Cells.Item(1).Specific;
                    oEdittext.Value = "A RECEBER";


                    oMatrix = oForm.Items.Item("178").Specific;
                    oColumm = oMatrix.Columns.Item("2002");
                    oEdittext = oColumm.Cells.Item(1).Specific;
                    oEdittext.Value = log;

                    oMatrix = oForm.Items.Item("178").Specific;
                    oColumm = oMatrix.Columns.Item("2");
                    oEdittext = oColumm.Cells.Item(1).Specific;
                    oEdittext.Value = rua;

                    oMatrix = oForm.Items.Item("178").Specific;
                    oColumm = oMatrix.Columns.Item("2003");
                    oEdittext = oColumm.Cells.Item(1).Specific;
                    oEdittext.Value = numero;


                    oMatrix = oForm.Items.Item("178").Specific;
                    oColumm = oMatrix.Columns.Item("2000");
                    oEdittext = oColumm.Cells.Item(1).Specific;
                    oEdittext.Value = comp;


                    oMatrix = oForm.Items.Item("178").Specific;
                    oColumm = oMatrix.Columns.Item("5");
                    oEdittext = oColumm.Cells.Item(1).Specific;
                    oEdittext.Value = cepresp;


                    oMatrix = oForm.Items.Item("178").Specific;
                    oColumm = oMatrix.Columns.Item("3");
                    oEdittext = oColumm.Cells.Item(1).Specific;
                    oEdittext.Value = bairro;





                    oMatrix = oForm.Items.Item("178").Specific;
                    oColumm = oMatrix.Columns.Item("4");
                    oEdittext = oColumm.Cells.Item(1).Specific;
                    oEdittext.Value = cidade;


                    oMatrix = oForm.Items.Item("178").Specific;
                    oComoBox = oMatrix.Columns.Item("7").Cells.Item(oMatrix.RowCount).Specific;
                    oComoBox.Select(uf, UI.BoSearchKey.psk_ByValue);



                    //ConectaSap();

                    //get DI company (via UI)

                    oCompany = (SAPbobsCOM.Company)SBO_Application.Company.GetDICompany();


                    SAPbobsCOM.Recordset RecPed = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    string qmunic = "SELECT T0.[AbsId] FROM OCNT T0 where t0.name = '" + cidade + "'";

                    RecPed.DoQuery(qmunic);

                    string cod = RecPed.Fields.Item("AbsId").Value.ToString();

                    if (RecPed.RecordCount > 0)

                    {

                        oMatrix = oForm.Items.Item("178").Specific;
                        oComoBox = oMatrix.Columns.Item("6").Cells.Item(oMatrix.RowCount).Specific;
                        oComoBox.Select(cod, UI.BoSearchKey.psk_ByValue);
                    }


                }

            }
            
        }

        private void SBO_Application_ItemEvent(string FormUID, ref UI.ItemEvent pVal, out bool BubbleEvent)
        {

            BubbleEvent = true;

            if (((pVal.FormType == 134 & pVal.EventType == UI.BoEventTypes.et_FORM_LOAD) & (pVal.Before_Action == false)))
            {

                //oForm = SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
                oForm = SBO_Application.Forms.GetForm("134", 1);
                oForm.DataSources.UserDataSources.Add("EditCNPJ", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 20);

                oItem = oForm.Items.Add("ConsCNPJ", UI.BoFormItemTypes.it_BUTTON);

                oItem.Left = 440;
                oItem.Width = 100;
                oItem.Top = 10;
                oItem.Height = 20;
                oItem.FromPane = 0;
                oItem.ToPane = 0;


                oButton = oItem.Specific;
                oButton.Caption = "Consultar";

                oItem = oForm.Items.Add("Cnpj", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 540;
                oItem.Width = 37;
                oItem.Top = 10;

                oItem.Height = 20;
                oItem.FromPane = 0;
                oItem.ToPane = 0;
                oItem.LinkTo = "TxtCnpj";
                oStaticText = ((SAPbouiCOM.StaticText)(oItem.Specific));
                oStaticText.Caption = "Cnpj :";


                /////EditTextcnpj
                oItem = oForm.Items.Add("TxtCnpj", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 590;
                oItem.Width = 159;
                oItem.Top = 10;
                oItem.Height = 20;
                oItem.FromPane = 0;
                oItem.ToPane = 0;
                oItem.LinkTo = "Cnpj";
                

          

                oEdittext = ((SAPbouiCOM.EditText)(oItem.Specific));

                // bind the text edit item to the defined used data source
                oEdittext.DataBind.SetBound(true, "", "EditCNPJ");

                oEdittext.Value = "Digite o CNPJ";








            }


            //https://archive.sap.com/discussions/thread/102952


        }

        private void SBO_Application_ConsultaCnae(string FormUID, ref UI.ItemEvent pVal, out bool BubbleEvent)
        {

            BubbleEvent = true;



            if ((pVal.FormTypeEx == "134") & (pVal.ItemUID == "1") & (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED) & (pVal.BeforeAction== false) & ( pVal.FormMode == 3 ))// & (pVal.BeforeAction == true) & ( pVal.ActionSuccess == true) )
            {


                CadastroFiscalPN(v_cnae, v_codpn, cnpj2, IE);
                

            }


        }


        public  void CadastroFiscalPN(string v_cnae,string v_codpn,string cnpj2, string IE)
        {

            oCompany = (Company)SBO_Application.Company.GetDICompany();

            SAPbobsCOM.Recordset RecCnae = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            string qnae = "SELECT T0.[AbsId] FROM OCNA T0 where t0.CNAECode = '" + v_cnae + "'";

            RecCnae.DoQuery(qnae);


            BusinessPartners oBusinessPartner = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

            oBusinessPartner.CardCode = v_codpn;
            oBusinessPartner.GetByKey(v_codpn);
            //oBusinessPartner.AliasName = "testes";
            //oBusinessPartner.FiscalTaxID.BPCode = ;
            oBusinessPartner.FiscalTaxID.CNAECode = Convert.ToInt32(RecCnae.Fields.Item("AbsID").Value.ToString());

            oBusinessPartner.FiscalTaxID.TaxId0 = Convert.ToString(cnpj2);

            oBusinessPartner.FiscalTaxID.TaxId1 = Convert.ToString(IE);


            oBusinessPartner.Valid = BoYesNoEnum.tYES;
            oBusinessPartner.Frozen = BoYesNoEnum.tNO;

            int resp = oBusinessPartner.Update();

            if (resp != 0)
            {
                string msgerro = oCompany.GetLastErrorDescription();
                SBO_Application.StatusBar.SetText(msgerro, UI.BoMessageTime.bmt_Long, UI.BoStatusBarMessageType.smt_Success);
            }
            else
            {
                SBO_Application.StatusBar.SetText("Dados Fiscais Atualizados com Sucesso", UI.BoMessageTime.bmt_Long, UI.BoStatusBarMessageType.smt_Success);

            }


        }


    }

}


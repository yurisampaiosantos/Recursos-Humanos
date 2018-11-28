<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Benefit.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #principal {
            background-color: #FFF;
            padding: 30px;
            height: auto;
            width: 900px;
            margin-top: 50px;
            margin-right: auto;
            margin-bottom: 50px;
            margin-left: auto;
        }

        #divMain {
            background-color: #CCC;
            margin: 0px;
            padding: 10px;
            height: 300px;
            width: 600px;
        }

        #divPlanoDeSaude {
            background-color: #999;
            margin-top: 20px;
            padding: inherit;
            height: 200px;
            width: 450px;
        }

        #divPlanoOdonto {
            background-color: #666666;
            margin: 0px;
            padding: inherit;
            height: 400px;
            width: 250px;
            border: 10px ;
            float: right;
        }
      
    </style>
</head>


<body>
    <form id="form1" runat="server">

        <div id="principal">

            <div style="height: 200px; width: 335px" id="divMain">


                <br />
                <br />
                <asp:Label ID="lbl_salary" runat="server" Text="Salário base:"></asp:Label>
                &nbsp;<asp:TextBox ID="txt_salary" runat="server"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="lbl_ir" runat="server" Text="Dependentes IR:"></asp:Label>
                <asp:TextBox ID="txt_ir" runat="server" Width="65px"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="lbl_nasc" runat="server" Text="Data de nascimento:"></asp:Label>
                <asp:TextBox ID="txt_nasc" runat="server" TextMode="Date"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="lbl_admissao" runat="server" Text="Data de admissão:"></asp:Label>
                <asp:TextBox ID="txt_admissao" runat="server" TextMode="Date"></asp:TextBox>


            </div>



            <div style="width: 280px; height: 192px" id="divPlanoDeSaude">
                <h2>Plano de Saúde</h2>
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" DataSourceID="dataSourceHealthInsurance" DataTextField="DisplayDescription" DataValueField="Amount">
                </asp:RadioButtonList>
                <asp:ObjectDataSource ID="dataSourceHealthInsurance" runat="server" SelectMethod="ListHealthInsurance" TypeName="DAO.benefit.HealthInsuranceDAO"></asp:ObjectDataSource>
            </div>



            <div style="width: 435px; height: 131px" id="divPlanoOdonto">
                <h2>Plano Odonto</h2>
                <asp:RadioButtonList ID="RadioButtonList2" runat="server" AutoPostBack="True" DataSourceID="dataSourceDentalInsurance" DataTextField="DisplayDscription" DataValueField="Amount">
                </asp:RadioButtonList>
                <asp:ObjectDataSource ID="dataSourceDentalInsurance" runat="server" SelectMethod="ListDentalInsurance" TypeName="DAO.benefit.DentalInsuranceDAO"></asp:ObjectDataSource>
            </div>




            <div style="width: 280px; height: 192px" id="divPrevComplementar">
                <h2>Previdencia complementar</h2>


                <asp:ObjectDataSource ID="dataSourceSupplementalInsurance" runat="server" SelectMethod="ListSupplementalInsurance" TypeName="DAO.benefit.SupplementalInsuranceDAO"></asp:ObjectDataSource>


                <asp:RadioButtonList ID="RadioButtonList3" runat="server" Height="72px" Width="181px" AutoPostBack="True" DataSourceID="dataSourceSupplementalInsurance" DataTextField="DisplayDescription" DataValueField="Amount"></asp:RadioButtonList>


            </div>

        </div>






    </form>

</body>
</html>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BUSINESS;
using Entities;
using PdfPrintingNet;

namespace TOTEM
{

    /// <summary>
    /// Enumerador para nomear os meses do ano
    /// </summary>
    enum Month
    {
        None = 00,
        Janeiro = 01,
        Fevereiro = 02,
        Março = 03,
        Abril = 04,
        Maio = 05,
        Junho = 06,
        Julho = 07,
        Agosto = 08,
        Setembro = 09,
        Outubro = 10,
        Novembro = 11,
        Dezembro = 12

    };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// CodeBehind da janela principal
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Objetos para utilização da view.
        /// </summary>
        Employee employeeVw = new Employee();
        HollerithBLL hollerithBll = new HollerithBLL();
        
        /// <summary>
        /// Objetos utilizados para impressão
        /// </summary>
        Hollerith hollerithImp = new Hollerith();
        HollerithFer hollerithImpFer = new HollerithFer();
        HollerithInf hollerithImpInf = new HollerithInf();

        HollerithImp printHollerith = new HollerithImp();

        /// <summary>
        /// Objeto para criação dos ítens de contra-cheques
        /// </summary>
        List<Hollerith> hollerithVwDem = new List<Hollerith>();

        /// <summary>
        /// Objeto para criação dos ítens de informe de rendimento
        /// </summary>
        List<HollerithInf> hollerithVwInf = new List<HollerithInf>();

        /// <summary>
        /// Objeto para criação dos ítens de Recibo de férias
        /// </summary>
        List<HollerithFer> hollerithVwFer = new List<HollerithFer>();

        /// <summary>
        /// Contador de itens.
        /// Métodos dentro de eventos de clique resetam-no.
        /// </summary>
        int numberofcalendars = 0;
        
        /// <summary>
        /// Este Token global vai indicar o item a ser impresso:
        /// </summary>
        string itemToBePrinted = "none";

        /// <summary>
        /// Construtor da janela principal
        /// Todas as rotinas contidas no construtor serão realizadas sempre
        /// que a janela for instânciada.
        /// </summary>
        /// <param name="user">Matricula do integrante no SAP</param>
        public MainWindow(string user) 
        {
            InitializeComponent();

            EmployeeBLL employeeBll = new EmployeeBLL();

            //verifica se o usuário é mão de obra direta
            int typeUserLogin = user.IndexOf(".");
            if (typeUserLogin == -1)
            {
                employeeVw = employeeBll.GetEmployee(user);
            }
            else
            {
               
                employeeVw = employeeBll.GetEmployeeAD(user);
                user = employeeVw.Sap_number;
            }

            //Tratammento do nome e sobrenome.
            string[] shortName =employeeVw.Name.ToString().Split(' ');
            this.lblName.Text = (shortName[0].ToString() + " " + shortName[shortName.Length - 1].ToString());
            this.lblSapNumber.Text = employeeVw.Sap_number.ToString();

            //Pega os últimos 6 objetos de demonstrativos dos tipos DEM/INF/FER
            hollerithVwDem = hollerithBll.getLastSixHollerithDem(user);
            hollerithVwInf = hollerithBll.getLastSixHollerithInf(user);
            hollerithVwFer = hollerithBll.getLastSixHollerithFer(user);

            //Carrega os ítens no primeiro acesso
            if (hollerithVwDem.Count() != 0)
            {
                ShowItems();
            }
            else
            {
                _createCalendarItem(9, "");
                numberofcalendars += 1;
                
                //refatorar
                // Animação dos ítens
                Storyboard fadeindemo = (Storyboard)FindResource("fadeindemo");
                Storyboard fadeoutinforme = (Storyboard)FindResource("fadeoutinforme");
                Storyboard fadeoutrecibo = (Storyboard)FindResource("fadeoutrecibo");
                fadeindemo.Begin(this);
                fadeoutinforme.Begin(this);
                fadeoutrecibo.Begin(this);
            }
            
        }

        /// <summary>
        /// Evita que o usuário utilize o ALT+F4 e as teclas de atalho para sair da aplicação.
        /// </summary>
        /// <param name="e">Evento</param>
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) && Keyboard.IsKeyDown(Key.F4))
                e.Handled = true;

            if ((Keyboard.IsKeyDown(Key.LeftCtrl)) && Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.X) && Keyboard.IsKeyDown(Key.A))
                e.Handled = true;

        }

        /// <summary>
        /// Método que cria os ítens para os demonstrativos de pagamento do tipo contra-cheque
        /// décimo terceiro e PLR.
        /// Chamado a partir do clique no ítem demonstrativo do menu.
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void Demonstrativo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Animação dos ítens
            Storyboard fadeindemo = (Storyboard)FindResource("fadeindemo");
            Storyboard fadeoutinforme = (Storyboard)FindResource("fadeoutinforme");
            Storyboard fadeoutrecibo = (Storyboard)FindResource("fadeoutrecibo");
            fadeindemo.Begin(this);
            fadeoutinforme.Begin(this);
            fadeoutrecibo.Begin(this);

            //Verifica o tipo do ítem para o método de impressão
            if (itemToBePrinted == "demonstrativo")
            { 
                // do nothing, since we have already clicked this button.
            }
            else
            {
                for (int counter = 0; counter < 1; counter++)
                {
                    Items.Children.RemoveAt(0);
                }

                if (hollerithVwDem.Count != 0)
                {
                    numberofcalendars = 0;
                    for (int counter = 0; counter < hollerithVwDem.Count; counter++)
                    {
                        _createCalendarItem(counter, "dem");
                        numberofcalendars += 1;
                    }
                }
                else
                {
                    //Aqui entra o que se deve fazer caso o integrante não tem contra-cheque disponível.
                    // workaround temporário: "9" é o código de situação vazia.
                    _createCalendarItem(9, ""); 
                    numberofcalendars += 1;
                }

                // Set a token stating that we're dealing with "Demonstrativos"
                itemToBePrinted = "demonstrativo";
            }
        }

        /// <summary>
        /// Método que cria os ítens para os demonstrativos de pagamento férias
        /// Chamado a partir do clique no ítem Recibo de férias do menu.
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void Recibo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Animação dos ítens
            Storyboard fadeoutdemo = (Storyboard)FindResource("fadeoutdemo");
            Storyboard fadeoutinforme = (Storyboard)FindResource("fadeoutinforme");
            Storyboard fadeinrecibo = (Storyboard)FindResource("fadeinrecibo");
            fadeoutdemo.Begin(this);
            fadeoutinforme.Begin(this);
            fadeinrecibo.Begin(this);

            //Verifica o tipo do ítem para o método de impressão
            if (itemToBePrinted == "recibo")
            {
                // Do nothing.
            }
            else
            {
                try
                {
                    for (int counter = 0; counter < numberofcalendars; counter++)
                    {
                        Items.Children.RemoveAt(0);
                    }
                }
                catch (ArgumentOutOfRangeException) { } // just to be sure.

                if (hollerithVwFer.Count != 0)
                {
                    numberofcalendars = 0;

                    for (int counter = 0; counter < hollerithVwFer.Count; counter++)
                    {
                        _createCalendarItem(counter, "fer");
                        numberofcalendars += 1;
                    }
                }
                else
                {
                    //Aqui entra o que se deve fazer caso o integrante não tem contra-cheque disponível.
                    // workaround temporário: "9" é o código de situação vazia.
                    _createCalendarItem(9, ""); 
                    numberofcalendars += 1;
                }

                // Set a token stating that we're dealing with "Recibo."
                itemToBePrinted = "recibo";
            }
        }

        /// <summary>
        /// Método que cria os ítens para os demonstrativos de pagamento informe de rendimento
        /// Chamado a partir do clique no ítem Informe de rendimento do menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Informe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Animação dos ítens
            Storyboard fadeoutdemo = (Storyboard)FindResource("fadeoutdemo");
            Storyboard fadeininforme = (Storyboard)FindResource("fadeininforme");
            Storyboard fadeoutrecibo = (Storyboard)FindResource("fadeoutrecibo");
            fadeoutdemo.Begin(this);
            fadeininforme.Begin(this);
            fadeoutrecibo.Begin(this);

            //Verifica o tipo do ítem para o método de impressão
            if (itemToBePrinted == "informe")
            {
                // Do nothing.
            }
            else
            {
                try
                {
                    for (int counter = 0; counter < numberofcalendars; counter++)
                    {
                        Items.Children.RemoveAt(0);
                    }
                }
                catch (ArgumentOutOfRangeException) { } // Just to be sure.

                numberofcalendars = 0;

                if (hollerithVwInf.Count != 0)
                {
                    numberofcalendars = 0;

                    for (int counter = 0; counter < hollerithVwInf.Count; counter++)
                    {
                        _createCalendarItem(counter, "inf");
                        numberofcalendars += 1;
                    }
                }
                else
                {
                    //Aqui entra o que se deve fazer caso o integrante não tem contra-cheque disponível.
                    // workaround temporário: "9" é o código de situação vazia.
                    _createCalendarItem(9, ""); 
                    numberofcalendars += 1;
                }

                // Set a token stating that we're dealing with "Informe"
                itemToBePrinted = "informe";
            }
        }

        /// <summary>
        /// Criação dos ítens (meses/ano)
        /// </summary>
        /// <param name="i">Número de iterações</param>
        /// <param name="numberMonth">Mês refente</param>
        private void _createCalendarItem(int i, string numberMonth)
        {
            // Label container
            Label mainSpawn = new Label();
          
            try
            {
                if (i == 9) // condição arbitrária que informa que não há itens
                {
                    mainSpawn.Name = "itemNoExhibition";
                    mainSpawn.Style = (Style)FindResource("itemNoExhibition");
                    mainSpawn.Content = "Não há itens para exibição.";
                }
                else
                {
                    string text = "";
                    string typeText = "";

                    //Verificação do mês e tipo para construção dos objetos e parâmetros necessários
                    //para impressão dos demonstrativos.
                    switch (numberMonth)
                    {                            
                        case "dem":

                            if (hollerithVwDem[i].Type == "DEM")
                            {
                                typeText = "DEM";
                                text = (Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwDem[i].Month))) + " " + hollerithVwDem[i].Year);
                                mainSpawn.Name = Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwDem[i].Month)));
                                hollerithImp = hollerithVwDem[i];
                                mainSpawn.Uid = hollerithImp.Month + " " + hollerithImp.Year + " " + hollerithImp.Type + " " + hollerithImp.Path;
                                break;
                            }
                            else
                            {
                                if (hollerithVwDem[i].Type == "PLR")
                                {
                                    typeText = "PLR";
                                    text = ("PLR " + hollerithVwDem[i].Month + "/" + hollerithVwDem[i].Year);
                                    mainSpawn.Name = Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwDem[i].Month)));
                                    hollerithImp = hollerithVwDem[i];
                                    mainSpawn.Uid = hollerithImp.Month + " " + hollerithImp.Year + " " + hollerithImp.Type + " " + hollerithImp.Path;
                                    break;
                                }
                                if (hollerithVwDem[i].Type == "DEC")
                                {
                                    
                                    
                                    
                                    mainSpawn.Name = Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwDem[i].Month)));
                                    hollerithImp = hollerithVwDem[i];
                                    mainSpawn.Uid = hollerithImp.Month + " " + hollerithImp.Year + " " + hollerithImp.Type + " " + hollerithImp.Path;

                                    // ISSUE #1: Se você tentar imprimir o recibo de férias, cancelar, e tentar imprimir um demonstrativo, a aplicação quebra.
                                    // A causa disso é que o nome do décimo-terceiro (mainSpawn.Name) está vindo em branco ("").
                                    // Temos que corrigí-lo:
                                    // MessageBox.Show(mainSpawn.Name); // One more debug.
                                    typeText = "DEC";
                                    if (hollerithImp.Month == "12")
                                    {
                                        text = ("2ª parcela \r Décimo terceiro " + hollerithVwDem[i].Year);
                                    }
                                    else
                                    {
                                        text = ("1ª parcela \r Décimo terceiro " + hollerithVwDem[i].Year);
                                    }

                                    break;
                                }
                                break;
                            }

                        case "fer":

                            typeText = "FER";
                            text = (Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwFer[i].Month))) + " " + hollerithVwFer[i].Year);
                            mainSpawn.Name = Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwFer[i].Month))) ;
                            hollerithImpFer = hollerithVwFer[i];
                            mainSpawn.Uid = hollerithImpFer.Month + " " + hollerithImpFer.Year + " " + hollerithImpFer.Type + " " + hollerithImpFer.Path;
                            break;

                        case "inf":

                            typeText = "INF";
                            text = (Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwInf[i].Month))) + " " + hollerithVwInf[i].Year);
                            mainSpawn.Name = Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwInf[i].Month))) ;
                            hollerithImpInf = hollerithVwInf[i];
                            mainSpawn.Uid = hollerithImpInf.Month + " " + hollerithImpInf.Year + " " + hollerithImpInf.Type + " " + hollerithImpInf.Path;
                            break;

                        default:
                            typeText = "DEM";
                            text = (Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwDem[i].Month))) + " " + hollerithVwDem[i].Year);
                            mainSpawn.Name = Enum.GetName(typeof(Month), Convert.ToInt32((hollerithVwDem[i].Month)));
                             hollerithImp = hollerithVwDem[i];
                            mainSpawn.Uid = hollerithImp.Month + " " + hollerithImp.Year + " " + hollerithImp.Type + " " + hollerithImp.Path;
                            break;
                    }

                    mainSpawn.Style = (Style)FindResource("item" + i); // <-- Just like here we will bind the animation to an x:Key

                    // Canvas to hold everything together
                    Canvas mainGrid = new Canvas();
                    mainGrid.Style = (Style)FindResource("calendariogrid");

                    // The text label
                    TextBlock mainContent = new TextBlock();
                    mainContent.Style = (Style)FindResource("calendariotext");
                    mainContent.Text = text;
                    mainContent.Name = typeText;

                    // The calendar icon overlay 
                    Image mainImage = new Image();
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.UriSource = new Uri("calendar.png", UriKind.Relative);
                    src.EndInit();
                    mainImage.Source = src;
                    mainImage.Style = (Style)FindResource("calendarioimage");

                    // Add content to the canvas and position everything nice ant neat.
                    mainGrid.Children.Add(mainImage);
                    mainGrid.Children.Add(mainContent);
                    mainSpawn.Content = mainGrid;

                    // Bind the event
                    mainGrid.MouseDown += ConfirmationDialog;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Se não houver nenhum item a ser exibido, mostre uma mensagem dizendo que não há itens a serem exibidos.
                mainSpawn.Name = "itemNoExhibition";
            }
                
            // Now append it to the body;
            Items.Children.Add(mainSpawn);

            // Animate the Opacity from 0 to 1:
            //Storyboard fadeInCalendarItem = (Storyboard)FindResource("fadeInCalendarItem");
            //fadeInCalendarItem.Begin(this);

            DoubleAnimation fadeInContent = new DoubleAnimation();
            //fadeInContent.From = 0;
            fadeInContent.To = 1;
            fadeInContent.Duration = new Duration(TimeSpan.FromMilliseconds(300));

            try
            {
                this.RegisterName(mainSpawn.Name, mainSpawn); // Fix this bug.
                Storyboard.SetTargetName(fadeInContent, mainSpawn.Name);
                Storyboard.SetTargetProperty(fadeInContent, new PropertyPath(Label.OpacityProperty));
                Storyboard fadeInStoryboard = new Storyboard();
                fadeInStoryboard.Children.Add(fadeInContent);
                fadeInStoryboard.Begin(mainSpawn);
                this.UnregisterName(mainSpawn.Name); // Esta linha traz alguns erros, vide ISSUE #1.
            }
            catch (ArgumentException)
            {
                // the problem is that when you go ahead and try to unregister mainSpawn.Name,
                // it is empty. 
                // If we do nothing here and let go, the elements won't appear correctly.
                MessageBox.Show("Um dos itens do calendário não apresentou um nome válido. O nome enviado foi uma empty string.");
            }
        }

        /// <summary>
        /// Confirmação de impressão
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void ConfirmationDialog(object sender, RoutedEventArgs e) 
        {
            // Walk UP the element hierarchy so we can choose the Label element that contains our data.
            FrameworkElement eventsource = e.Source as FrameworkElement; // TextBlock
            FrameworkElement theCanvas = eventsource.Parent as FrameworkElement; // Canvas
            FrameworkElement dataHolder = theCanvas.Parent as FrameworkElement; // <Label> - stop walking.

            //Adicionando valor do mês ao label
            string monthText;
            TextBlock t1 = (TextBlock)e.Source;
            monthText = t1.Text;
            string[] monthYearTypePath;
            monthYearTypePath = dataHolder.Uid.Split(' ');
            printHollerith.Cod_sap = employeeVw.Sap_number;
            printHollerith.Month = monthYearTypePath[0];
            printHollerith.Year = monthYearTypePath[1];
            printHollerith.Type = monthYearTypePath[2];
            printHollerith.Path = monthYearTypePath[3];

            // Create the shade:
            Canvas shade = new Canvas();
            shade.Height = 1200;
            shade.Width = 2000;
            shade.Background = (Brush)new BrushConverter().ConvertFrom("#C3FFFFFF");
            shade.Name = "shade";

            // Create the Dialog box:
            Label ConfirmPrint = new Label();
            ConfirmPrint.Style = (Style)FindResource("promptDialog");

            // Very clever casting hack to obtain window size - ANY window.
            // Unfortunately, it doesn't do very well concerning small screens (1024px width)...
            // We can work around it by making a condition:
            int workableArea = int.Parse((((Grid)this.Content).RenderSize.Width - 550).ToString());
            if (workableArea > 1024)
            {
                Canvas.SetLeft(ConfirmPrint, (((Grid)this.Content).RenderSize.Width - 550) / 2);
                Canvas.SetTop(ConfirmPrint, (((Grid)this.Content).RenderSize.Height - 240) / 2);
            }
            else
            {
                Canvas.SetLeft(ConfirmPrint, 237);
                Canvas.SetTop(ConfirmPrint, (((Grid)this.Content).RenderSize.Height - 240) / 2);
            }

            Grid positions = new Grid();
            positions.Style = (Style)FindResource("promptGrid");
            
            // Positioning guidelines
            RowDefinition spacetop = new RowDefinition();
            spacetop.Height = new GridLength(15);
            RowDefinition title = new RowDefinition();
            title.Height = new GridLength(45);
            RowDefinition printitem = new RowDefinition();
            printitem.Height = new GridLength(30);
            RowDefinition printdate = new RowDefinition();
            printdate.Height = new GridLength(30);
            RowDefinition spacer = new RowDefinition();
            spacer.Height = new GridLength(35);
            RowDefinition buttons = new RowDefinition();
            buttons.Height = new GridLength(60);
            RowDefinition spacebottom = new RowDefinition();
            spacebottom.Height = new GridLength(15);
            ColumnDefinition spaceleft = new ColumnDefinition();
            spaceleft.Width = new GridLength(20);
            ColumnDefinition colleft = new ColumnDefinition();
            colleft.Width = new GridLength(200);
            ColumnDefinition spacerhor = new ColumnDefinition();
            spacerhor.Width = new GridLength(100);
            ColumnDefinition colright = new ColumnDefinition();
            colright.Width = new GridLength(200);
            ColumnDefinition spaceright = new ColumnDefinition();
            spaceright.Width = new GridLength(20);

            positions.RowDefinitions.Add(spacetop);
            positions.RowDefinitions.Add(title);
            positions.RowDefinitions.Add(printitem);
            positions.RowDefinitions.Add(printdate);
            positions.RowDefinitions.Add(spacer);
            positions.RowDefinitions.Add(buttons);
            positions.RowDefinitions.Add(spacebottom);
            positions.ColumnDefinitions.Add(spaceleft);
            positions.ColumnDefinitions.Add(colleft);
            positions.ColumnDefinitions.Add(spacerhor);
            positions.ColumnDefinitions.Add(colright);
            positions.ColumnDefinitions.Add(spaceright);

            // Create the overlay
            TextBlock prompt = new TextBlock();
            prompt.Style = (Style)FindResource("promptTitle");
            prompt.Text = "Deseja imprimir?"; 
            TextBlock itemname = new TextBlock();
            itemname.Style = (Style)FindResource("promptItemName");
            
            switch(itemToBePrinted) 
            {
                case "demonstrativo":
                    itemname.Text = "Demonstrativo de Pagamento";
                    break;
                case "recibo":
                    itemname.Text = "Recibo de Férias";
                    break;
                case "informe":
                    itemname.Text = "Informe de Rendimento";
                    break;
                default:
                    itemname.Text = "Demonstrativo de Pagamento"; // By default it will be clicked anyway.
                    break;
            }
            TextBlock itemdate = new TextBlock();
            itemdate.Style = (Style)FindResource("promptItemDate");
            itemdate.Text = (monthText); 
            Label buttonYes = new Label();
            buttonYes.Style = (Style)FindResource("promptBtnYes");
            buttonYes.Content = "Sim";
            Label buttonNo = new Label();
            buttonNo.Style = (Style)FindResource("promptBtnNo");
            buttonNo.Content = "Não";

            Image printIcon = new Image();
            BitmapImage s = new BitmapImage();
            s.BeginInit();
            s.CacheOption = BitmapCacheOption.OnLoad;
            s.UriSource = new Uri("printer.png", UriKind.Relative);
            s.EndInit();
            printIcon.Source = s;
            printIcon.Width = 200;
            printIcon.Style = (Style)FindResource("promptIcon");

            // Pack everything
            positions.Children.Add(prompt);
            positions.Children.Add(itemname);
            positions.Children.Add(itemdate);
            positions.Children.Add(printIcon);
            positions.Children.Add(buttonYes);
            positions.Children.Add(buttonNo);

            Grid.SetRow(prompt, 1);
            Grid.SetRow(itemname, 2);
            Grid.SetRow(itemdate, 3);
            Grid.SetRow(buttonYes, 5);
            Grid.SetRow(buttonNo, 5);
            Grid.SetRow(printIcon, 0);

            Grid.SetColumn(prompt, 1);
            Grid.SetColumn(itemname, 1);
            Grid.SetColumn(itemdate, 1);
            Grid.SetColumn(buttonYes, 1);
            Grid.SetColumn(buttonNo, 3);
            Grid.SetColumn(printIcon, 3);

            Grid.SetColumnSpan(prompt, 2);
            Grid.SetColumnSpan(itemname, 2);
            Grid.SetColumnSpan(itemdate, 2);
            Grid.SetRowSpan(printIcon, 6);
            Grid.SetColumnSpan(printIcon, 5);
            
            // Drop the shade:
            ConfirmPrint.Content = positions;
            shade.Children.Add(ConfirmPrint);
            main.Children.Add(shade);

            // Animate the shade:
            DoubleAnimation fadeInContent = new DoubleAnimation();
            fadeInContent.From = 0;
            fadeInContent.To = 1;
            fadeInContent.Duration = new Duration(TimeSpan.FromMilliseconds(300));

            this.RegisterName(shade.Name, shade);
            Storyboard.SetTargetName(fadeInContent, shade.Name);
            Storyboard.SetTargetProperty(fadeInContent, new PropertyPath(Label.OpacityProperty));
            Storyboard fadeInStoryboard = new Storyboard();
            fadeInStoryboard.Children.Add(fadeInContent);
            fadeInStoryboard.Begin(shade);
            this.UnregisterName(shade.Name);

            // Bind the "Não" button to return to the screen
            buttonNo.MouseDown += CloseConfirmationDialog;

            //Confirm print "SIM"
            buttonYes.MouseDown += PrintConfirmationDialog;
        }

        /// <summary>
        /// Método de impressão
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void PrintConfirmationDialog(object sender, MouseButtonEventArgs e)
        {
            try
            {
                PrintFile printFile = new PrintFile();
                bool printResult = false;
                //hollerithImp.Cod_sap = hollerithImp.Cod_sap + hollerithImp.Month + hollerithImp.Year + hollerithImp.Type;
                printHollerith.Cod_sap = printHollerith.Cod_sap + printHollerith.Month + printHollerith.Year + printHollerith.Type;
                printResult = printFile.printFileLocalWithPath(printHollerith.Cod_sap, printHollerith.Path, " ");
                if (printResult == true)
                {
                    SuccessMessage(); // Já sai da aplicação por padrão.
                }
                else
                {
                    main.Children.RemoveAt(4);
                    ErrorMessage();
                }
            }
            catch (Exception) 
            {
                main.Children.RemoveAt(4);
                ErrorMessage();
            }
        }

        /// <summary>
        /// Fecha a caixa de diálogo
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void CloseConfirmationDialog(object sender, RoutedEventArgs e)
        {
            // Needs to fadeOut before removal.
            main.Children.RemoveAt(4); // The 4th child is always the shade.
        }
        /// <summary>
        /// Caixa de diálogo para sair da aplicação
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void quitter_MouseDown(object sender, MouseButtonEventArgs e)
        {

            // Create the shade:
            Canvas shade = new Canvas();
            shade.Height = 1200;
            shade.Width = 2000;
            shade.Background = (Brush)new BrushConverter().ConvertFrom("#C3FFFFFF");
            shade.Name = "shade";

            // Create the Dialog box:
            Label ConfirmPrint = new Label();
            ConfirmPrint.Style = (Style)FindResource("promptDialog");
            Canvas.SetLeft(ConfirmPrint, (((Grid)this.Content).RenderSize.Width - 550) / 2); // Very clever casting hack to obtain window size - ANY window.
            // debugging for the sake of correctness:
            // MessageBox.Show(Canvas.GetLeft(ConfirmPrint).ToString());
            Canvas.SetTop(ConfirmPrint, (((Grid)this.Content).RenderSize.Height - 240) / 2);

            Grid positions = new Grid();
            positions.Style = (Style)FindResource("promptGrid");

            // Positioning guidelines
            RowDefinition spacetop = new RowDefinition();
            spacetop.Height = new GridLength(15);
            RowDefinition title = new RowDefinition();
            title.Height = new GridLength(45);
            RowDefinition printitem = new RowDefinition();
            printitem.Height = new GridLength(30);
            RowDefinition printdate = new RowDefinition();
            printdate.Height = new GridLength(30);
            RowDefinition spacer = new RowDefinition();
            spacer.Height = new GridLength(35);
            RowDefinition buttons = new RowDefinition();
            buttons.Height = new GridLength(60);
            RowDefinition spacebottom = new RowDefinition();
            spacebottom.Height = new GridLength(15);

            ColumnDefinition spaceleft = new ColumnDefinition();
            spaceleft.Width = new GridLength(20);
            ColumnDefinition colleft = new ColumnDefinition();
            colleft.Width = new GridLength(200);
            ColumnDefinition spacerhor = new ColumnDefinition();
            spacerhor.Width = new GridLength(100);
            ColumnDefinition colright = new ColumnDefinition();
            colright.Width = new GridLength(200);
            ColumnDefinition spaceright = new ColumnDefinition();
            spaceright.Width = new GridLength(20);

            positions.RowDefinitions.Add(spacetop);
            positions.RowDefinitions.Add(title);
            positions.RowDefinitions.Add(printitem);
            positions.RowDefinitions.Add(printdate);
            positions.RowDefinitions.Add(spacer);
            positions.RowDefinitions.Add(buttons);
            positions.RowDefinitions.Add(spacebottom);
            positions.ColumnDefinitions.Add(spaceleft);
            positions.ColumnDefinitions.Add(colleft);
            positions.ColumnDefinitions.Add(spacerhor);
            positions.ColumnDefinitions.Add(colright);
            positions.ColumnDefinitions.Add(spaceright);

            // Create the overlay
            TextBlock prompt = new TextBlock();
            prompt.Style = (Style)FindResource("promptTitle");
            prompt.Text = "Deseja sair?";
            TextBlock itemname = new TextBlock();
            itemname.Style = (Style)FindResource("promptItemName");
            itemname.Text = " ";
            TextBlock itemdate = new TextBlock();
            itemdate.Style = (Style)FindResource("promptItemDate");
            itemdate.Text = " ";
            Label buttonYes = new Label();
            buttonYes.Style = (Style)FindResource("promptBtnYes");
            buttonYes.Content = "Sim";
            Label buttonNo = new Label();
            buttonNo.Style = (Style)FindResource("promptBtnNo");
            buttonNo.Content = "Não";
            Image printIcon = new Image();
            BitmapImage s = new BitmapImage();
            s.BeginInit();
            s.CacheOption = BitmapCacheOption.OnLoad;
            s.UriSource = new Uri("question.png", UriKind.Relative);
            s.EndInit();
            printIcon.Source = s;
            printIcon.Width = 180;
            printIcon.Style = (Style)FindResource("promptIcon");

            // Pack everything
            positions.Children.Add(prompt);
            positions.Children.Add(itemname);
            positions.Children.Add(itemdate);
            positions.Children.Add(printIcon);
            positions.Children.Add(buttonYes);
            positions.Children.Add(buttonNo);

            Grid.SetRow(prompt, 1);
            Grid.SetRow(itemname, 2);
            Grid.SetRow(itemdate, 3);
            Grid.SetRow(buttonYes, 5);
            Grid.SetRow(buttonNo, 5);
            Grid.SetRow(printIcon, 0);

            Grid.SetColumn(prompt, 1);
            Grid.SetColumn(itemname, 1);
            Grid.SetColumn(itemdate, 1);
            Grid.SetColumn(buttonYes, 1);
            Grid.SetColumn(buttonNo, 3);
            Grid.SetColumn(printIcon, 2);

            Grid.SetColumnSpan(prompt, 2);
            Grid.SetColumnSpan(itemname, 2);
            Grid.SetColumnSpan(itemdate, 2);
            Grid.SetRowSpan(printIcon, 6);
            Grid.SetColumnSpan(printIcon, 5);

            // Drop the shade:
            ConfirmPrint.Content = positions;
            shade.Children.Add(ConfirmPrint);
            main.Children.Add(shade);

            // Animate the shade:
            DoubleAnimation fadeInContent = new DoubleAnimation();
            fadeInContent.From = 0;
            fadeInContent.To = 1;
            fadeInContent.Duration = new Duration(TimeSpan.FromMilliseconds(300));

            this.RegisterName(shade.Name, shade);
            Storyboard.SetTargetName(fadeInContent, shade.Name);
            Storyboard.SetTargetProperty(fadeInContent, new PropertyPath(Label.OpacityProperty));
            Storyboard fadeInStoryboard = new Storyboard();
            fadeInStoryboard.Children.Add(fadeInContent);

            fadeInStoryboard.Begin(shade);
            this.UnregisterName(shade.Name);

            // Bind the "Não" button to return to the screen
            buttonNo.MouseDown += CloseConfirmationDialog;

            // Confirm logout
            buttonYes.MouseDown += _logout;
        }

        /// <summary>
        /// Logout do sistema. Voltar para a tela de login
        /// </summary>
        /// <param name="sender">Evento</param>
        /// <param name="e">Evento</param>
        private void _logout(object sender, MouseButtonEventArgs e)
        {
            // Actually logging out without confirmation
            Window loginScreen = new VIEW.LoginScreen();
            loginScreen.Show();
            Application.Current.Windows[0].Close();
        }

        /// <summary>
        /// Mostra os ítens de demonstrativos. 
        /// Utilizado para mostrar os ítens, apos o login.
        /// </summary>
        private void ShowItems()
        {
            Storyboard fadeindemo = (Storyboard)FindResource("fadeindemo");
            Storyboard fadeoutinforme = (Storyboard)FindResource("fadeoutinforme");
            Storyboard fadeoutrecibo = (Storyboard)FindResource("fadeoutrecibo");
            fadeindemo.Begin(this);
            fadeoutinforme.Begin(this);
            fadeoutrecibo.Begin(this);

            //===============================================================================================
            // O trecho abaixo só ilustra a criação de ítens de calendário para um número arbitrário (seis).
            // Na realidade, teríamos que iterar sobre os tantos objetos quantos forem retornados pelo SAP.
            //===============================================================================================
            itemToBePrinted = "demonstrativo";
            if (hollerithVwDem.Count != 0)
            {
                numberofcalendars = 0;

                for (int counter = 0; counter < hollerithVwDem.Count; counter++)
                {
                    _createCalendarItem(counter, "dem");
                    numberofcalendars += 1;
                }
            }
            else
            {
                //Aqui entra o que se deve fazer caso o integrante não tem contra-cheque disponível.
                _createCalendarItem(9, ""); // workaround temporário: "9" é o código de situação vazia.
                numberofcalendars += 1;
            }
        }

        /// <summary>
        /// One last time for the customized error messages!
        /// Caixa de dialogo para mensagem de erro. Padrão para todos os erros.
        /// Requisito: não passar o tipo de erro para o usuário.
        /// </summary>
        private void ErrorMessage()
        {
            
            /// Mensagem de erro customizada para atender os padrões da interface que criamos
            /// (All in all, just another big PR stunt)

            // Create the shade:
            Canvas shade = new Canvas();
            shade.Height = 1200;
            shade.Width = 2000;
            shade.Background = (Brush)new BrushConverter().ConvertFrom("#C3FFFFFF");
            shade.Name = "shade";

            // Create the Dialog box:
            Label ConfirmPrint = new Label();
            ConfirmPrint.Style = (Style)FindResource("promptDialog");
            Canvas.SetLeft(ConfirmPrint, (((Grid)this.Content).RenderSize.Width - 550) / 2); // Very clever casting hack to obtain window size - ANY window.
            Canvas.SetTop(ConfirmPrint, (((Grid)this.Content).RenderSize.Height - 240) / 2);

            Grid positions = new Grid();
            positions.Style = (Style)FindResource("promptGrid");


            // Positioning guidelines
            RowDefinition spacetop = new RowDefinition();
            spacetop.Height = new GridLength(15);
            RowDefinition title = new RowDefinition();
            title.Height = new GridLength(45);
            RowDefinition printitem = new RowDefinition();
            printitem.Height = new GridLength(30);
            RowDefinition printdate = new RowDefinition();
            printdate.Height = new GridLength(30);
            RowDefinition spacer = new RowDefinition();
            spacer.Height = new GridLength(35);
            RowDefinition buttons = new RowDefinition();
            buttons.Height = new GridLength(60);
            RowDefinition spacebottom = new RowDefinition();
            spacebottom.Height = new GridLength(15);

            ColumnDefinition spaceleft = new ColumnDefinition();
            spaceleft.Width = new GridLength(20);
            ColumnDefinition colleft = new ColumnDefinition();
            colleft.Width = new GridLength(200);
            ColumnDefinition spacerhor = new ColumnDefinition();
            spacerhor.Width = new GridLength(100);
            ColumnDefinition colright = new ColumnDefinition();
            colright.Width = new GridLength(200);
            ColumnDefinition spaceright = new ColumnDefinition();
            spaceright.Width = new GridLength(20);

            positions.RowDefinitions.Add(spacetop);
            positions.RowDefinitions.Add(title);
            positions.RowDefinitions.Add(printitem);
            positions.RowDefinitions.Add(printdate);
            positions.RowDefinitions.Add(spacer);
            positions.RowDefinitions.Add(buttons);
            positions.RowDefinitions.Add(spacebottom);

            positions.ColumnDefinitions.Add(spaceleft);
            positions.ColumnDefinitions.Add(colleft);
            positions.ColumnDefinitions.Add(spacerhor);
            positions.ColumnDefinitions.Add(colright);
            positions.ColumnDefinitions.Add(spaceright);

            // Create the overlay
            TextBlock prompt = new TextBlock();
            prompt.Style = (Style)FindResource("promptTitle");
            prompt.Text = "Erro ao imprimir"; // PR STUNT!!!
            TextBlock itemname = new TextBlock();
            itemname.Style = (Style)FindResource("ErrorText");
            itemname.Text = "Um erro ocorreu e não foi possível imprimir o documento."; // PR STUNT!!!
            TextBlock itemdate = new TextBlock();
            itemdate.Style = (Style)FindResource("ErrorText");
            itemdate.Text = "Por favor, tente novamente ou entre em contato com a equipe de suporte."; // PR STUNT!!!

            Label buttonNo = new Label();
            buttonNo.Style = (Style)FindResource("promptBtnNo");
            buttonNo.Content = "Ok";

            Image printIcon = new Image();
            BitmapImage s = new BitmapImage();
            s.BeginInit();
            s.CacheOption = BitmapCacheOption.OnLoad;
            s.UriSource = new Uri("error.png", UriKind.Relative);
            s.EndInit();
            printIcon.Source = s;
            printIcon.Width = 180;
            printIcon.Style = (Style)FindResource("promptIcon");

            // Pack everything
            positions.Children.Add(prompt);
            positions.Children.Add(printIcon);
            positions.Children.Add(itemname);
            positions.Children.Add(itemdate);
            positions.Children.Add(buttonNo);

            Grid.SetRow(prompt, 1);
            Grid.SetRow(itemname, 2);
            Grid.SetRow(itemdate, 3);
            Grid.SetRow(buttonNo, 5);
            Grid.SetRow(printIcon, 0);

            Grid.SetColumn(prompt, 1);
            Grid.SetColumn(itemname, 1);
            Grid.SetColumn(itemdate, 1);
            Grid.SetColumn(buttonNo, 3);
            Grid.SetColumn(printIcon, 2);

            Grid.SetColumnSpan(prompt, 2);
            Grid.SetColumnSpan(itemname, 3);
            Grid.SetColumnSpan(itemdate, 3);
            Grid.SetRowSpan(printIcon, 6);
            Grid.SetRowSpan(itemdate, 2);
            Grid.SetColumnSpan(printIcon, 5);

            // Drop the shade:
            ConfirmPrint.Content = positions;
            shade.Children.Add(ConfirmPrint);
            main.Children.Add(shade);

            // Animate the shade:
            DoubleAnimation fadeInContent = new DoubleAnimation();
            fadeInContent.From = 0;
            fadeInContent.To = 1;
            fadeInContent.Duration = new Duration(TimeSpan.FromMilliseconds(300));

            this.RegisterName(shade.Name, shade);
            Storyboard.SetTargetName(fadeInContent, shade.Name);
            Storyboard.SetTargetProperty(fadeInContent, new PropertyPath(Label.OpacityProperty));
            Storyboard fadeInStoryboard = new Storyboard();
            fadeInStoryboard.Children.Add(fadeInContent);

            fadeInStoryboard.Begin(shade);
            this.UnregisterName(shade.Name);

            // Bind OK ro return to screen.
            buttonNo.MouseDown += CloseConfirmationDialog;
        }


        /// <summary>
        /// Caixa de diálogo informando ao Usuário que a impressão foi concluída
        /// com sucesso. 
        /// Requisito: esta mensagem segura a execução - o usuário deve pressionar
        /// OK para continuar - e leva direto de volta para a tela inicial (no "seconds")
        /// </summary>
        private void SuccessMessage()
        {

            // Create the shade:
            Canvas shade = new Canvas();
            shade.Height = 1200;
            shade.Width = 2000;
            shade.Background = (Brush)new BrushConverter().ConvertFrom("#C3FFFFFF");
            shade.Name = "shade";

            // Create the Dialog box:
            Label ConfirmPrint = new Label();
            ConfirmPrint.Style = (Style)FindResource("promptDialog");
            Canvas.SetLeft(ConfirmPrint, (((Grid)this.Content).RenderSize.Width - 550) / 2); // Very clever casting hack to obtain window size - ANY window.
            Canvas.SetTop(ConfirmPrint, (((Grid)this.Content).RenderSize.Height - 240) / 2);

            Grid positions = new Grid();
            positions.Style = (Style)FindResource("promptGrid");


            // Positioning guidelines
            RowDefinition spacetop = new RowDefinition();
            spacetop.Height = new GridLength(15);
            RowDefinition title = new RowDefinition();
            title.Height = new GridLength(45);
            RowDefinition printitem = new RowDefinition();
            printitem.Height = new GridLength(30);
            RowDefinition printdate = new RowDefinition();
            printdate.Height = new GridLength(30);
            RowDefinition spacer = new RowDefinition();
            spacer.Height = new GridLength(35);
            RowDefinition buttons = new RowDefinition();
            buttons.Height = new GridLength(60);
            RowDefinition spacebottom = new RowDefinition();
            spacebottom.Height = new GridLength(15);

            ColumnDefinition spaceleft = new ColumnDefinition();
            spaceleft.Width = new GridLength(20);
            ColumnDefinition colleft = new ColumnDefinition();
            colleft.Width = new GridLength(200);
            ColumnDefinition spacerhor = new ColumnDefinition();
            spacerhor.Width = new GridLength(100);
            ColumnDefinition colright = new ColumnDefinition();
            colright.Width = new GridLength(200);
            ColumnDefinition spaceright = new ColumnDefinition();
            spaceright.Width = new GridLength(20);

            positions.RowDefinitions.Add(spacetop);
            positions.RowDefinitions.Add(title);
            positions.RowDefinitions.Add(printitem);
            positions.RowDefinitions.Add(printdate);
            positions.RowDefinitions.Add(spacer);
            positions.RowDefinitions.Add(buttons);
            positions.RowDefinitions.Add(spacebottom);

            positions.ColumnDefinitions.Add(spaceleft);
            positions.ColumnDefinitions.Add(colleft);
            positions.ColumnDefinitions.Add(spacerhor);
            positions.ColumnDefinitions.Add(colright);
            positions.ColumnDefinitions.Add(spaceright);

            // Create the overlay
            TextBlock prompt = new TextBlock();
            prompt.Style = (Style)FindResource("promptTitle");
            prompt.Text = "Impressão concluída";
            TextBlock itemname = new TextBlock();
            itemname.Style = (Style)FindResource("ErrorText");
            itemname.Text = "";
            TextBlock itemdate = new TextBlock();
            itemdate.Style = (Style)FindResource("ErrorText");
            itemdate.Text = "";

            Label buttonNo = new Label();
            buttonNo.Style = (Style)FindResource("promptBtnNo");
            buttonNo.Content = "Ok";

            Image printIcon = new Image();
            BitmapImage s = new BitmapImage();
            s.BeginInit();
            s.CacheOption = BitmapCacheOption.OnLoad;
            s.UriSource = new Uri("success.png", UriKind.Relative);
            s.EndInit();
            printIcon.Source = s;
            printIcon.Width = 180;
            printIcon.Style = (Style)FindResource("promptIcon");

            // Pack everything
            positions.Children.Add(prompt);
            positions.Children.Add(printIcon);
            positions.Children.Add(itemname);
            positions.Children.Add(itemdate);
            positions.Children.Add(buttonNo);

            Grid.SetRow(prompt, 1);
            Grid.SetRow(itemname, 2);
            Grid.SetRow(itemdate, 3);
            Grid.SetRow(buttonNo, 5);
            Grid.SetRow(printIcon, 0);

            Grid.SetColumn(prompt, 1);
            Grid.SetColumn(itemname, 1);
            Grid.SetColumn(itemdate, 1);
            Grid.SetColumn(buttonNo, 3);
            Grid.SetColumn(printIcon, 2);

            Grid.SetColumnSpan(prompt, 3);
            Grid.SetColumnSpan(itemname, 3);
            Grid.SetColumnSpan(itemdate, 3);
            Grid.SetRowSpan(printIcon, 6);
            Grid.SetRowSpan(itemdate, 2);
            Grid.SetColumnSpan(printIcon, 5);

            // Drop the shade:
            ConfirmPrint.Content = positions;
            shade.Children.Add(ConfirmPrint);
            main.Children.Add(shade);

            // Animate the shade:
            DoubleAnimation fadeInContent = new DoubleAnimation();
            fadeInContent.From = 0;
            fadeInContent.To = 1;
            fadeInContent.Duration = new Duration(TimeSpan.FromMilliseconds(300));

            this.RegisterName(shade.Name, shade);
            Storyboard.SetTargetName(fadeInContent, shade.Name);
            Storyboard.SetTargetProperty(fadeInContent, new PropertyPath(Label.OpacityProperty));
            Storyboard fadeInStoryboard = new Storyboard();
            fadeInStoryboard.Children.Add(fadeInContent);

            fadeInStoryboard.Begin(shade);
            this.UnregisterName(shade.Name);

            // Bind OK ro return to screen.
            buttonNo.MouseDown += _logout;
        }

        

    }


}


/*
 * How does the animation system work?
 * 
 * The animation procedure (ex. what to do, how to do it...) must be set within the
 * .xaml file itself, in a storyboard in the <Window.Resourcarrees> tag and carrying an 
 * x:Key for identification.
 * 
 * Once that's done, you must come to this codebehind file and define an object to be
 * the storyboard. Then you start the animation by calling Object.Begin()
 */


/// Acredito que, para atingir os objetivos que queremos, devemos
/// iterá-la para todos os objetos de dados que houverem , criando assim
/// até seis itens de calendarios (escopo do projeto)
/// 
/// A estrutura do item de calendário é:
/// 
/// <Label>
///     <Grid>
///         <Image />
///         <TextBlock />
///     </Grid>
/// </Label>
/// 
/// UPDATE: istead of <Grid>, which causes elements to split rows, we now
/// mash up everything in a nice <Canvas>. Reasons for this includes:
/// 
/// 1. The canvas element allows for element overlapping without caring 
///    about row/column snapping.
/// 
/// 2. Each calendar item will have fixed dimension constraints, so absolute
///    positioning will not be too unpredictable (we can center through hard
///    constraints, for example).
/// 
/// In summary, the new structure is:
/// 
/// <Label>
///     <Canvas>                                             _____________
///         <Image /> ------+--> Absolute positioning       /            /
///         <TextBlock/> ---+                              /  PICTURE   /
///     </Canvas>                                         /            /
/// </Label>                                             /    TEXT    /
///                                                     /____________/
///                                                     

///Confirmation Dialog
///
// Proof-of-concept of a minimal dialog box
//MessageBox.Show("Deseja imprimir " + sender.ToString() + "?", "", MessageBoxButton.YesNo);

/// Para gerar o menu de confirmação, injetaremos as seguintes camadas na interface:
/// 
/// <Canvas Width="2000" Height="1200" Background="#C3FFFFFF">
///     <Grid VerticalAlignment="Center">
///         <Grid.RowDefinitions>
///             <RowDefinition Height="auto" />
///             <RowDefinition Height="300" />
///             <RowDefinition Height="auto" />
///         </Grid.RowDefinitions>
///         <Grid.ColumnDefinitions>
///             <ColumnDefinition Width="auto" />
///             <ColumnDefinition Width="700" />
///             <ColumnDefinition Width="auto" />
///         </Grid.ColumnDefinitions>
///     </Grid>
/// </Canvas>
/// 



/// <summary>
/// Método para loader de impressão. Em teste.
/// Não utilizado.
/// </summary>
//private void Loader()
//{

//    /// Mensagem indicando algum processo em background acontecendo em paralelo.
//    /// Não possui nenhum botão, meramente informativo.
//    /// Deve ser removido assim que o processo termina.

//    // Create the shade:
//    Canvas shade = new Canvas();
//    shade.Height = 1200;
//    shade.Width = 2000;
//    shade.Background = (Brush)new BrushConverter().ConvertFrom("#C3FFFFFF");
//    shade.Name = "shade";

//    // Create the Dialog box:
//    Label ConfirmPrint = new Label();
//    ConfirmPrint.Style = (Style)FindResource("promptDialog");
//    Canvas.SetLeft(ConfirmPrint, (((Grid)this.Content).RenderSize.Width - 550) / 2); // Very clever casting hack to obtain window size - ANY window.
//    Canvas.SetTop(ConfirmPrint, (((Grid)this.Content).RenderSize.Height - 240) / 2);

//    Grid positions = new Grid();
//    positions.Style = (Style)FindResource("promptGrid");


//    // Positioning guidelines
//    RowDefinition spacetop = new RowDefinition();
//    spacetop.Height = new GridLength(15);
//    RowDefinition title = new RowDefinition();
//    title.Height = new GridLength(45);
//    RowDefinition printitem = new RowDefinition();
//    printitem.Height = new GridLength(30);
//    RowDefinition printdate = new RowDefinition();
//    printdate.Height = new GridLength(30);
//    RowDefinition spacer = new RowDefinition();
//    spacer.Height = new GridLength(35);
//    RowDefinition buttons = new RowDefinition();
//    buttons.Height = new GridLength(60);
//    RowDefinition spacebottom = new RowDefinition();
//    spacebottom.Height = new GridLength(15);

//    ColumnDefinition spaceleft = new ColumnDefinition();
//    spaceleft.Width = new GridLength(20);
//    ColumnDefinition colleft = new ColumnDefinition();
//    colleft.Width = new GridLength(200);
//    ColumnDefinition spacerhor = new ColumnDefinition();
//    spacerhor.Width = new GridLength(100);
//    ColumnDefinition colright = new ColumnDefinition();
//    colright.Width = new GridLength(200);
//    ColumnDefinition spaceright = new ColumnDefinition();
//    spaceright.Width = new GridLength(20);

//    positions.RowDefinitions.Add(spacetop);
//    positions.RowDefinitions.Add(title);
//    positions.RowDefinitions.Add(printitem);
//    positions.RowDefinitions.Add(printdate);
//    positions.RowDefinitions.Add(spacer);
//    positions.RowDefinitions.Add(buttons);
//    positions.RowDefinitions.Add(spacebottom);

//    positions.ColumnDefinitions.Add(spaceleft);
//    positions.ColumnDefinitions.Add(colleft);
//    positions.ColumnDefinitions.Add(spacerhor);
//    positions.ColumnDefinitions.Add(colright);
//    positions.ColumnDefinitions.Add(spaceright);

//    // Create the overlay
//    TextBlock prompt = new TextBlock();
//    prompt.Style = (Style)FindResource("promptTitle");
//    prompt.Text = "Imprimindo...";

//    Image printIcon = new Image();
//    BitmapImage s = new BitmapImage();
//    s.BeginInit();
//    s.CacheOption = BitmapCacheOption.OnLoad;
//    s.UriSource = new Uri("printer.png", UriKind.Relative);
//    s.EndInit();
//    printIcon.Source = s;
//    printIcon.Width = 180;
//    printIcon.Style = (Style)FindResource("promptIcon");

//    // Pack everything
//    positions.Children.Add(prompt);
//    positions.Children.Add(printIcon);

//    Grid.SetRow(prompt, 1);
//    Grid.SetRow(printIcon, 0);

//    Grid.SetColumn(prompt, 1);
//    Grid.SetColumn(printIcon, 2);

//    Grid.SetColumnSpan(prompt, 2);
//    Grid.SetRowSpan(printIcon, 6);
//    Grid.SetColumnSpan(printIcon, 5);

//    // Drop the shade:
//    ConfirmPrint.Content = positions;
//    shade.Children.Add(ConfirmPrint);
//    main.Children.Add(shade);

//    // Animate the shade:
//    DoubleAnimation fadeInContent = new DoubleAnimation();
//    fadeInContent.From = 0;
//    fadeInContent.To = 1;
//    fadeInContent.Duration = new Duration(TimeSpan.FromMilliseconds(300));

//    this.RegisterName(shade.Name, shade);
//    Storyboard.SetTargetName(fadeInContent, shade.Name);
//    Storyboard.SetTargetProperty(fadeInContent, new PropertyPath(Label.OpacityProperty));
//    Storyboard fadeInStoryboard = new Storyboard();
//    fadeInStoryboard.Children.Add(fadeInContent);

//    fadeInStoryboard.Begin(shade);
//    this.UnregisterName(shade.Name);
//}
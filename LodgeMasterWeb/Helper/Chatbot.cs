using Newtonsoft.Json;
using System.Text;

namespace LodgeMasterWeb.Helper;

public class Chatbot
{
    private readonly ApplicationDbContext _context;
    private readonly HttpContext _httpContext;
    public Chatbot(ApplicationDbContext context, HttpContext httpContext)
    {
        _context = context;
        _httpContext = httpContext;
    }

    public async Task StartChatbotMessage(string flowchartId, int QustionNo)
    {
        try
        {

            var _CompanyID = _httpContext.Session.GetString("CompanyID");
            var _CompanyFolder = _httpContext.Session.GetString("CompanyFolder");
            var _UserID = _httpContext.Session.GetString("UserID");


            //var StartChart = await _context.FlowChartDetails
            //        .Where(detail =>  detail.FlowchartID == flowchartId)
            //        .Join(
            //            _context.FlowChartMasters.Where(master => master.CompanyID == _CompanyID),
            //            detail => detail.FlowchartID,
            //            master => master.FlowchartID,
            //            (detail, master) => new
            //            {
            //                master.FlowchartName,
            //                detail.FCDetailsID,
            //                detail.FlowchartID,
            //                detail.HeaderMessage_E,
            //                detail.BodyMessage_E,
            //                detail.FooterMessage_E,
            //                detail.HeaderMessage_A,
            //                detail.BodyMessage_A,
            //                detail.FooterMessage_A,
            //                detail.MultiAnwser,
            //                detail.ActionAnwser,
            //                detail.CondationAnwser,
            //                detail.bActive,
            //                detail.Sorted,
            //                master.CompanyID,
            //                MasterActiveStatus = master.bActive
            //            })
            //        .ToListAsync(); 

            var StartChart = await _context.FlowChartDetails
                    .FirstOrDefaultAsync(detail => detail.FlowchartID == flowchartId && detail.Sorted == QustionNo);

            string strFCDetailsID = "";
            string strHeaderMsg = "";
            string strBodyMsg = "";
            string strFooterMsg = "";
            string strButton = "";

            if (StartChart != null)
            {
                strFCDetailsID = StartChart.FCDetailsID;

                strHeaderMsg = StartChart.HeaderMessage_E;
                strBodyMsg = StartChart.BodyMessage_E;
                strFooterMsg = StartChart.FooterMessage_E;
                //strButton = StartChart.HeaderMessage_E;

                if (StartChart.MultiAnwser != -1)
                {
                    var FCAction = await _context.FlowChartActions
                            .Where(c => c.FCDetailsID == strFCDetailsID)
                            .OrderBy(c => c.Sorted)
                            .ToListAsync();

                    if (FCAction != null)
                    {
                        foreach (var action in FCAction)
                        {
                            strButton += flowchartId + "\"," + action.DisplayAnswer;
                        }

                    }

                }
                else
                {
                    strButton = StartChart.CondationAnwser;
                }

            }

        }
        catch (Exception)
        {


        }

    }
    public async Task SendWhatsAppMessageAsync(string jsonTemplate)
    {
        var client = new HttpClient();
        var content = new StringContent(jsonTemplate, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://graph.facebook.com/v12.0/{your_whatsapp_number_id}/messages", content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Message sent successfully.");
        }
        else
        {
            Console.WriteLine("Failed to send message.");
        }
    }
    public async Task<string> GenerateTemplate()
    {
        try
        {
            var template = new WhatsAppTemplate
            {
                Name = "create_order",
                LanguageCode = "en_US",
                Components = new List<Component>
            {
                new Component
                {
                    Type = "header",
                    Parameters = new List<Parameter>
                    {
                        new Parameter { Type = "text", Text = "Order Confirmation" }
                    }
                },
                new Component
                {
                    Type = "body",
                    Parameters = new List<Parameter>
                    {
                        new Parameter { Type = "text", Text = "Thank you for your order. Your order number is {{order_no}}." }
                    }
                },
                new Component
                {
                    Type = "button",
                    Parameters = new List<Parameter>
                    {
                        new Parameter { Type = "text", Text = "Track Order" }
                    }
                }
            }
            };

            string jsonTemplate = JsonConvert.SerializeObject(template, Formatting.Indented);
            return jsonTemplate;
            //Console.WriteLine(jsonTemplate);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public class FlowChartMaster
    {
        public string FlowchartID { get; set; }
        public string FlowchartName { get; set; }
        public string CompanyID { get; set; }
        public bool bActive { get; set; }
        public ICollection<FlowChartDetail> FlowChartDetails { get; set; }
    }

    public class FlowChartDetail
    {
        public string FCDetailsID { get; set; }
        public string FlowchartID { get; set; }
        public string HeaderMessage_E { get; set; }
        public string BodyMessage_E { get; set; }
        public string FooterMessage_E { get; set; }
        public string HeaderMessage_A { get; set; }
        public string BodyMessage_A { get; set; }
        public string FooterMessage_A { get; set; }
        public bool MultiAnwser { get; set; }
        public bool ActionAnwser { get; set; }
        public bool CondationAnwser { get; set; }
        public bool bActive { get; set; }
        public int Sorted { get; set; }

        public FlowChartMaster FlowChartMaster { get; set; }
    }

    public class WhatsAppTemplate
    {
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public string Key { get; set; }
        public List<Component> Components { get; set; }
    }

    public class Component
    {
        public string Type { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Key { get; set; }
    }

    public class Button
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Key { get; set; }
    }
}
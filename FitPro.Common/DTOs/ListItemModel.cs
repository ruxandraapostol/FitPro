namespace FitPro.Common
{
    public class ListItemModel<TText, TValue>
    {
        public TText Text { get; set; }
        public TValue Value { get; set; }
        public bool Selected { get; set; }
    }
}

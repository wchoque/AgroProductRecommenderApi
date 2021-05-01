namespace AppCentroIdiomas.Models
{
    public class NoteModel
    {
        public int Id { get; set; }
        public string EvaluationName { get; set; }
        public int EvaluationWeightPercentage { get; set; }
        public decimal? Value { get; set; }
    }
}

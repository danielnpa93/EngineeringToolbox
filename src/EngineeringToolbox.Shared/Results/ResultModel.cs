namespace EngineeringToolbox.Shared.Results
{
    public class ResultModel
    {
        public string? DisplayMessage { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public bool Success { get; set; }
    }

    public class ResultModel<TData> : ResultModel
    {
        public TData Data { get; set; }
    }
}

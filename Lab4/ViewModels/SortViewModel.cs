namespace Lab4.ViewModels
{
    public enum SortState
    {
        No, // не сортировать
        NameAsc, // по имени по возрастанию
        NameDesc, // по имени по убыванию
        DescriptionAsc, // по описанию по возрастанию
        DescriptionDesc, // по описанию по убыванию
        CostAsc, // по стоимости по возрастанию
        CostDesc, // по стоимости по убыванию
        DateAsc, // по дате по возрастанию
        DateDesc // по дате по убыванию
    }

    public class SortViewModel
    {
        public SortState NameSort { get; set; } // значение для сортировки по имени
        public SortState DescriptionSort { get; set; } // значение для сортировки по описанию
        public SortState CostSort { get; set; } // значение для сортировки по стоимости
        public SortState DateSort { get; set; } // значение для сортировки по дате
        public SortState CurrentState { get; set; } // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            DescriptionSort = sortOrder == SortState.DescriptionAsc ? SortState.DescriptionDesc : SortState.DescriptionAsc;
            CostSort = sortOrder == SortState.CostAsc ? SortState.CostDesc : SortState.CostAsc;
            DateSort = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            CurrentState = sortOrder;
        }
    }
}

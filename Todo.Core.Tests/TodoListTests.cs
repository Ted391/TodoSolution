namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }
        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }
        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }
        [Fact]
        public void SaveAndLoad_PersistsTasks()
        {
            var originalList = new TodoList();
            originalList.Add("Купить хлеб");
            var itemToComplete = originalList.Add("Сделать домашнее задание");
            itemToComplete.MarkDone();

            string tempFilePath = Path.GetTempFileName();

            try
            {
                originalList.Save(tempFilePath);

                var loadedList = new TodoList();
                loadedList.Load(tempFilePath);

                Assert.Equal(originalList.Count, loadedList.Count);

                var originalItem = originalList.Items.First(i => i.Title == "Сделать домашнее задание");
                var loadedItem = loadedList.Items.FirstOrDefault(i => i.Id == originalItem.Id);

                Assert.NotNull(loadedItem);
                Assert.Equal(originalItem.Title, loadedItem.Title);
                Assert.Equal(originalItem.IsDone, loadedItem.IsDone);
                Assert.True(loadedItem.IsDone);
            }
            finally
            {
                // Очистка
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }
    }
}
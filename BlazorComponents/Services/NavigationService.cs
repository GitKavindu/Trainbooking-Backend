namespace BlazorComponents.Service
{
    public class NavigationService<T>
    {
        private List<T> _itemsArr;
        public int _selectedPage;
        private int _pageSize;

        public NavigationService(List<T> arr)
        {
            this._itemsArr = arr;
            this._selectedPage = 1;
            this._pageSize = 5;
        }

        public int GetMaxPageNumber()
        {
            if (_itemsArr.Count % _pageSize == 0)
                return _itemsArr.Count / _pageSize;
            else
                return (_itemsArr.Count / _pageSize) + 1;
        }

        public void PageForward()
        {
            if (_selectedPage <= GetMaxPageNumber() - 1)
            {
                _selectedPage++;
            }
        }

        public void PageBackward()
        {
            if (_selectedPage > 1)
            {
                _selectedPage--;
            }
        }

        public bool IsRowVisible(int index)
        {
            if ((index / _pageSize) == (_selectedPage - 1))
                return true;
            else
                return false;
        }

        public List<T> GetVisibleRows()
        {
            int firstIndex = _pageSize * (_selectedPage - 1);
            int lastIndex;

            if (firstIndex + _pageSize > _itemsArr.Count)
            {
                lastIndex = _itemsArr.Count;
            }
            else
            {
                lastIndex = firstIndex + _pageSize;
            }

            // GetRange takes (startIndex, count)
            return _itemsArr.GetRange(firstIndex, lastIndex - firstIndex);
        }

        public int getRealRowNum(int rowNo){
            return this._pageSize * (this._selectedPage - 1) + rowNo;
        }

    }
}
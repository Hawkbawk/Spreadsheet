using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS9
{
    class BoggleClientController
    {
        IBoggleService view;

        public BoggleClientController(IBoggleService _view)
        {
            view = _view;

            view.EnterGame += HandleEnterGame;
            view.CancelGame += HandleCancelGame;
            view.SubmitWord += HandleSubmitWord;

        }

        private void HandleSubmitWord(string obj)
        {
            throw new NotImplementedException();
        }

        private void HandleCancelGame()
        {
            throw new NotImplementedException();
        }

        private void HandleEnterGame()
        {
            throw new NotImplementedException();
        }
    }
}

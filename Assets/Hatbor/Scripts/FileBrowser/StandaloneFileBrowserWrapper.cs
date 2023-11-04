using Cysharp.Threading.Tasks;
using Hatbor.UI;

namespace Hatbor.Scripts.FileBrowser
{
    public sealed class StandaloneFileBrowserWrapper : IFileBrowser
    {
        public UniTask<string> ChooseFileAsync(string extension)
        {
            var t = new UniTaskCompletionSource<string>();
            SFB.StandaloneFileBrowser.OpenFilePanelAsync("", "", extension, false,
                paths =>
                {
                    t.TrySetResult(paths.Length > 0 ? paths[0] : "");
                });
            return t.Task;
        }
    }
}
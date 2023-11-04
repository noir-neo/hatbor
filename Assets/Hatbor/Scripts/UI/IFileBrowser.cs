using Cysharp.Threading.Tasks;

namespace Hatbor.UI
{
    public interface IFileBrowser
    {
        UniTask<string> ChooseFileAsync(string extension);
    }
}
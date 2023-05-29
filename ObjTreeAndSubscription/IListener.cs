namespace Lab8_oop
{
    public interface IListener
    {
        void OnMasterChanged(IMaster obj);
    }
    public interface IMaster
    {
        void Notify();
        void AddListener(IListener obj);
    }
}

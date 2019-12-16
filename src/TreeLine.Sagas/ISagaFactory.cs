namespace TreeLine.Sagas
{
    public interface ISagaFactory
    {
        public ISaga<TProfile> Create<TProfile>() where TProfile : ISagaProfile;
    }
}
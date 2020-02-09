﻿using TreeLine.Sagas.Building;

namespace TreeLine.Sagas
{
    public interface ISagaProfile
    {
        void Configure(ISagaProcessorBuilder processorBuilder);
    }
}
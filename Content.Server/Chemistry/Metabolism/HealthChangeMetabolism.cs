using Content.Server.GameObjects.Components.Nutrition;
using Content.Shared.Chemistry;
using Content.Shared.Damage;
using Content.Shared.GameObjects.Components.Damage;
using Content.Shared.Interfaces.Chemistry;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Serialization;
using Robust.Shared.Serialization;

namespace Content.Server.Chemistry.Metabolism
{
    /// <summary>
    /// Default metabolism for medicine reagents. Attempts to find a DamegableComponent on the target,
    /// and to update it's damage values.
    /// </summary>
    class HealthChangeMetabolism : IMetabolizable
    {
        //Rate of metabolism in units / second
        private ReagentUnit _metabolismRate;
        public ReagentUnit MetabolismRate => _metabolismRate;


        //How much damage is changed when 1u of the reagent is metabolized
        private float _healthChangePerSec;
        private DamageClass _healthChangeType;
        private DamageClass HealthChangeType => _healthChangeType;
        public float HealthChangePerSec => _healthChangePerSec;

        void IExposeData.ExposeData(ObjectSerializer serializer)
        {
            serializer.DataField(ref _metabolismRate, "rate", ReagentUnit.New(1));
            serializer.DataField(ref _healthChangePerSec, "healFactor", 0f);
            serializer.DataField(ref _healthChangeType, "healType", DamageClass.Brute);
        }

        //Remove reagent at set rate, changes damage if a DamageableComponent can be found
        ReagentUnit IMetabolizable.Metabolize(IEntity solutionEntity, string reagentId, float tickTime)
        {
            var metabolismAmount = MetabolismRate * tickTime;

            if (solutionEntity.TryGetComponent(out DamageableComponent health))

                health.ChangeDamage(HealthChangeType, (int) (metabolismAmount.Float() * HealthChangePerSec), true);

            return metabolismAmount;
        }
    }
}

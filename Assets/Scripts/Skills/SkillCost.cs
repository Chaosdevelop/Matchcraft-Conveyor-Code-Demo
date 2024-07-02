using System;

namespace Skills
{
	/// <summary>
	/// Represents the cost management for a skill, including energy and charges.
	/// </summary>
	public class SkillCost
	{
		public int EnergyPerCharge { get; private set; }
		public int MaxCharges { get; private set; }
		public int CurrentEnergy { get; private set; }
		public int MaxEnergy => MaxCharges * EnergyPerCharge;

		public int StartEnergy { get; set; }

		public event Action OnChargesChanged;
		public event Action OnEnergyChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="SkillCost"/> class.
		/// </summary>
		/// <param name="energyPerCharge">The energy required per charge.</param>
		/// <param name="maxCharges">The maximum number of charges.</param>
		public SkillCost(int energyPerCharge, int maxCharges)
		{
			EnergyPerCharge = energyPerCharge;
			MaxCharges = maxCharges;
			CurrentEnergy = 0;
		}

		/// <summary>
		/// Gets the current number of charges based on the current energy.
		/// </summary>
		public int CurrentCharges => CurrentEnergy / EnergyPerCharge;

		/// <summary>
		/// Spends one charge if available.
		/// </summary>
		public void SpendCharge()
		{
			if (CurrentCharges > 0)
			{
				CurrentEnergy -= EnergyPerCharge;
				NotifyChargesChanged();
				NotifyEnergyChanged();
			}
		}

		/// <summary>
		/// Adds energy to the current energy.
		/// </summary>
		/// <param name="energy">The amount of energy to add.</param>
		public void AddEnergy(int energy)
		{
			CurrentEnergy += energy;
			if (CurrentEnergy > MaxEnergy)
			{
				CurrentEnergy = MaxEnergy;
			}
			NotifyChargesChanged();
			NotifyEnergyChanged();
		}

		/// <summary>
		/// Adds additional charges to the maximum limit.
		/// </summary>
		/// <param name="additionalLimit">The number of additional charges to add.</param>
		public void AddMaxCharges(int additionalLimit)
		{
			MaxCharges += additionalLimit;
		}

		/// <summary>
		/// Notifies subscribers that the charges have changed.
		/// </summary>
		private void NotifyChargesChanged()
		{
			OnChargesChanged?.Invoke();
		}

		/// <summary>
		/// Notifies subscribers that the energy has changed.
		/// </summary>
		private void NotifyEnergyChanged()
		{
			OnEnergyChanged?.Invoke();
		}

		/// <summary>
		/// Resets the energy to the starting energy value.
		/// </summary>
		public void ResetEnergy()
		{
			CurrentEnergy = StartEnergy;
			NotifyEnergyChanged();
			NotifyChargesChanged();
		}
	}
}

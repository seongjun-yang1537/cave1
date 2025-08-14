using NUnit.Framework;
using UnityEngine;
using Ingame;
using UnityEditor;

public class AgentModelTests
{
    [Test]
    public void OnHeldItem_IsCalled_WhenInventoryIsUpdated()
    {
        // Arrange
        var agentData = ScriptableObject.CreateInstance<AgentModelData>();
        agentData.baseStat = new AgentStat();
        var agentModel = new AgentModel(agentData);
        var itemData = ScriptableObject.CreateInstance<ItemModelData>();
        var item = ItemModelFactory.Create(itemData);

        var heldSlot = agentModel.inventory.GetItemSlot(QuickSlotID.Slot1);
        agentModel.SetHeldItem(heldSlot);

        bool onHeldItemCalled = false;
        agentModel.onHeldItem.AddListener((_) => { onHeldItemCalled = true; });

        // Act
        agentModel.inventory.SetItemSlot(QuickSlotID.Slot1, item);

        // Assert
        Assert.IsTrue(onHeldItemCalled, "onHeldItem event should be called when the held item slot is updated.");
    }
}

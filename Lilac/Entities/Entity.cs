using System.Collections.Generic;
using Lilac.Components;

namespace Lilac.Entities;

public abstract class Entity
{
	private readonly List<IComponent> components = new();
	
	/// <summary>Add a new <see cref="IComponent"/> to the Entity if the specified component type isn't already present.</summary>
	/// <returns><see langword="true"/> if the component was added successfully; <see langword="false"/> otherwise.</returns>
	protected bool AddComponent(IComponent component)
	{
		foreach (var existingComponent in components)
		{
			if (existingComponent.GetType() == component.GetType())
				return false;
		}

		components.Add(component);
		return true;
	}

	/// <summary>Searches for an existing <see cref="IComponent"/> in the Entity.</summary>
	/// <returns>The matching component, if found; <see langword="default"/> otherwise.</returns>
	public T? GetComponent<T>() where T : IComponent
	{
		foreach (var component in components)
		{
			if (component is T match)
				return match;
		}
		
		return default;
	}

	/// <summary>Searches for an existing <see cref="IComponent"/> in the Entity, and removes it if present.</summary>
	/// <returns><see langword="true"/> if the component was removed successfully; <see langword="false"/> otherwise.</returns>
	protected bool RemoveComponent<T>() where T : IComponent
	{
		for (var i = 0; i < components.Count; i++)
		{
			if (components[i] is not T)
				continue;

			components.RemoveAt(i);
			return true;
		}
		
		return false;
	}

	public abstract void Render();
}
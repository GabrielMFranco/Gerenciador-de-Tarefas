using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App_Gerenciador_de_Tarefas.Foundation;

/// <summary>
/// Base <see cref="INotifyPropertyChanged" /> abstract implementation using weak event pattern.
/// </summary>
/// <remarks>
///		This implementation of <see cref="INotifyPropertyChanged"/> uses the weak event pattern to implement <see cref="INotifyPropertyChanged.PropertyChanged"/>, so subscribers will not be hold alive by their subscription if they don't correctly unsubscribe.
/// </remarks>
/// <seealso cref="INotifyPropertyChanged" />
public abstract class ObservableObject : INotifyPropertyChanged {
	/// <summary>
	/// Weak event that occurs when the value of a property has changed.
	/// </summary>
	public event PropertyChangedEventHandler PropertyChanged;

	private readonly ConcurrentDictionary<string, object> _propertyValues = new();

	/// <summary>
	/// Sets a property, even if it didn't changed, using an automatic backing field (requires calling <see cref="Get{TProperty}(string)"/> on the property getter) and notifies via <see cref="INotifyPropertyChanged.PropertyChanged"/>.
	/// </summary>
	/// <typeparam name="TProperty">Type of the property.</typeparam>
	/// <param name="newValue">The new value to set.</param>
	/// <param name="propertyName">Name of the property (if called from a setter, don't inform this parameter).</param>
	/// <returns>True if the property has changed.</returns>
	protected void ForceSet<TProperty>(TProperty newValue, [CallerMemberName] string propertyName = "") => InternalSet(newValue, null, propertyName, true);
	/// <summary>
	/// Notify to <see cref="INotifyPropertyChanged.PropertyChanged"/> subscribers that a property value as changed.
	/// </summary>
	/// <param name="propertyName">Name of the property (if called from a setter, don't inform this parameter).</param>
	protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
	/// <summary>
	/// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
	/// </summary>
	/// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => RaisePropertyChanged(e);
	/// <summary>
	/// Sets a property using the specified backing field and notifies if the property value has changed via <see cref="INotifyPropertyChanged.PropertyChanged"/>.
	/// </summary>
	/// <typeparam name="TProperty">Type of the property.</typeparam>
	/// <param name="backingField">The property backing field.</param>
	/// <param name="newValue">The new value to set.</param>
	/// <param name="equalityComparer">Expression to use to check if the new value is equals to the older.</param>
	/// <param name="propertyName">Name of the property (if called from a setter, don't inform this parameter).</param>
	/// <returns>True if the property has changed.</returns>
	protected bool Set<TProperty>(ref TProperty backingField, TProperty newValue, IEqualityComparer<TProperty> equalityComparer = null, [CallerMemberName] string propertyName = "") {
		if (InternalSet(ref backingField, newValue, equalityComparer, false)) {
			NotifyPropertyChanged(propertyName);
			return true;
		}
		return false;
	}
	/// <summary>
	/// Sets a property using an automatic backing field (requires calling <see cref="Get{TProperty}(string)"/> on the property getter) and notifies if the property value has changed via <see cref="INotifyPropertyChanged.PropertyChanged"/>.
	/// </summary>
	/// <typeparam name="TProperty">Type of the property.</typeparam>
	/// <param name="newValue">The new value to set.</param>
	/// <param name="equalityComparer">Expression to use to check if the new value is equals to the older.</param>
	/// <param name="propertyName">Name of the property (if called from a setter, don't inform this parameter).</param>
	/// <returns>True if the property has changed.</returns>
	protected bool Set<TProperty>(TProperty newValue, IEqualityComparer<TProperty> equalityComparer = null, [CallerMemberName] string propertyName = "") => InternalSet(newValue, equalityComparer, propertyName, false);
	/// <summary>
	/// Gets the specified property value from its automatic backing field (requires calling <see cref="Set{TProperty}(TProperty, IEqualityComparer{TProperty}, string)"/> on property setter).
	/// </summary>
	/// <typeparam name="TProperty">The type of the property.</typeparam>
	/// <param name="propertyName">Name of the property.</param>
	/// <returns>The stored property value.</returns>
	protected TProperty Get<TProperty>([CallerMemberName] string propertyName = "") => _propertyValues.TryGetValue(propertyName, out object value) ? (TProperty)value : default;
	/// <summary>
	/// Gets the specified property value from its automatic backing field (requires calling <see cref="Set{TProperty}(TProperty, IEqualityComparer{TProperty}, string)"/> on property setter).
	/// </summary>
	/// <param name="propertyName">Name of the property.</param>
	/// <returns>The stored property value.</returns>
	protected object Get([CallerMemberName] string propertyName = "") => Get<object>(propertyName);

	private static bool InternalSet<TProperty>(ref TProperty backingField, TProperty newValue, IEqualityComparer<TProperty> equalityComparer, bool ignoreEqualityComparer) {
		if (ignoreEqualityComparer || !(equalityComparer ?? EqualityComparer<TProperty>.Default).Equals(backingField, newValue)) {
			backingField = newValue;
			return true;
		}
		return false;
	}

	private bool InternalSet<TProperty>(TProperty newValue, IEqualityComparer<TProperty> equalityComparer, string propertyName, bool ignoreEqualityComparer) {
		TProperty backingField = _propertyValues.TryGetValue(propertyName, out object currentValue) ? (TProperty)currentValue : default;
		if (InternalSet(ref backingField, newValue, equalityComparer, ignoreEqualityComparer)) {
			_propertyValues.AddOrUpdate(propertyName, newValue, (_, _) => newValue);
			NotifyPropertyChanged(propertyName);
			return true;
		}
		return false;
	}
	private void RaisePropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
}
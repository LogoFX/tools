using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LogoFX.Client.Mvvm.Model;
using LogoFX.Tools.TemplateGenerator.Model.Contract;

namespace LogoFX.Tools.TemplateGenerator.Model
{
    internal abstract class AppModel : EditableModel<Guid>, IAppModel
    {
        public virtual bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }

            oldValue = newValue;
            NotifyOfPropertyChange(propertyName ?? string.Empty);
            
            return true;
        }
    }
}
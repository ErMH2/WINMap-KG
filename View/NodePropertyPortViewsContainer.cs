using NodeGraph.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NodeGraph.View
{
	public class NodePropertyPortViewsContainer : ItemsControl
	{
		#region Fields

		private Type _ViewType = null;

		#endregion // Fields

		#region Properties

		public bool IsInput
		{
			get { return ( bool )GetValue( IsInputProperty ); }
			set { SetValue( IsInputProperty, value ); }
		}
		public static readonly DependencyProperty IsInputProperty =
			DependencyProperty.Register( "IsInput", typeof( bool ), typeof( NodePropertyPortViewsContainer ), new PropertyMetadata( false ) );

		#endregion // Properties

		#region Overrides ItemsControl

		protected override bool IsItemItsOwnContainerOverride( object item )
		{
			NodePropertyPortViewModel viewModel = item as NodePropertyPortViewModel;

			var attrs = item.GetType().GetCustomAttributes( typeof( NodePropertyPortViewModelAttribute ), false ) as NodePropertyPortViewModelAttribute[];

			if( 0 == attrs.Length )
			{
				throw new Exception( "A NodePropertyPortViewModelAttribute must exist for NodePropertyPortViewModel class." );
			}
			else if( 1 < attrs.Length )
			{
				throw new Exception( "A NodePropertyPortViewModelAttribute must exist only one." );
			}

			_ViewType = attrs[ 0 ].ViewType;

			return base.IsItemItsOwnContainerOverride( item );
		}

		protected override void PrepareContainerForItemOverride( DependencyObject element, object item )
		{
			base.PrepareContainerForItemOverride( element, item );

			var attrs = item.GetType().GetCustomAttributes( typeof( NodePropertyPortViewModelAttribute ), false ) as NodePropertyPortViewModelAttribute[];

			if( 0 == attrs.Length )
			{
				throw new Exception( "A NodePropertyPortViewModelAttribute must exist for NodePropertyPortViewModel class." );
			}
			else if( 1 < attrs.Length )
			{
				throw new Exception( "A NodePropertyPortViewModelAttribute must exist only one." );
			}

			FrameworkElement fe = element as FrameworkElement;

			ResourceDictionary resourceDictionary = new ResourceDictionary
			{
				Source = new Uri( "/NodeGraph;component/Themes/generic.xaml", UriKind.RelativeOrAbsolute )
			};

			Style style = resourceDictionary[ attrs[ 0 ].ViewStyleName ] as Style;
			if( null == style )
			{
				style = Application.Current.TryFindResource( attrs[ 0 ].ViewStyleName ) as Style;
			}
			fe.Style = style;

			if( null == fe.Style )
				throw new Exception( String.Format( "{0} does not exist", attrs[ 0 ].ViewStyleName ) );
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return Activator.CreateInstance( _ViewType, new object[]{ IsInput } ) as DependencyObject;
		}

		#endregion // Overrides ItemsControl
	}
}

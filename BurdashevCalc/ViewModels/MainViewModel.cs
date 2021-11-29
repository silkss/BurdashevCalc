using BurdashevCalc.Infrastructure;

namespace BurdashevCalc.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public Values ValuesVM { get; }
        public Result ResultVM { get; }
        public MainViewModel()
        {
            ValuesVM = new Values();
            ResultVM = new Result(ValuesVM);
            Calc = new LambdaCommand(onCalcExecuted, _ => true);
        }


        public LambdaCommand Calc { get; }
        private void onCalcExecuted(object p)
        {
            ResultVM.CalcResult();
        }
    }

    /// <summary>
    /// Все необходимые параметры.
    /// Размеры будки X Y Z все указывается в милиметрах
    /// Размеры профиля Z Y, тоже в милиметрах.
    /// </summary>
    public class Values: ViewModel
    {
        public Cabin CabinSizes { get; }
        public Profile ProfileSizes { get; }
        public Values()
        {
            CabinSizes = new Cabin();
            ProfileSizes = new Profile();
        }

    }
    #region Cabin
    /// <summary>
    /// Все размеры кабины в мм.
    /// </summary>
    public class Cabin : ViewModel 
    {
        
        private int Xvalue;
        public int X
        {
            get => Xvalue;
            set => Set(ref Xvalue, value);
        }

        private int Yvalue;
        public int Y
        {
            get => Yvalue;
            set => Set(ref Yvalue, value);
        }

        private int Zvalue;
        public int Z
        {
            get => Zvalue;
            set => Set(ref Zvalue, value);
        }


        private int YsegmentsInXSurfaceValue;
        public int YsegmentsInXSurface
        {
            get => YsegmentsInXSurfaceValue;
            set => Set(ref YsegmentsInXSurfaceValue, value);
        }

        private int YsegmentsInZSurfaceValue;
        public int YsegmentsInZsurface
        {
            get => YsegmentsInZSurfaceValue;
            set => Set(ref YsegmentsInZSurfaceValue, value);
        }
    }
    #endregion

    #region Profile
    /// <summary>
    /// Все размеры профиля в мм
    /// </summary>
    public class Profile: ViewModel
    {
        #region Размеры профиля
        private int Yvalue = 20;
        public int Y
        {
            get => Yvalue;
            set => Set(ref Yvalue, value);
        }
        private int Zvalue = 20;
        public int Z
        {
            get => Zvalue;
            set => Set(ref Zvalue, value);
        }
        #endregion
    }
    #endregion

    public class Result : ViewModel
    {
        private readonly Values sizes;
        public Result(Values Sizes)
        {
            sizes = Sizes;
        }

        /// <summary>
        /// Возвращает количество ребер по оси Х
        /// их всегда будет 4
        /// </summary>
        public int SegmentXQuantity => 4;
        
        /// <summary>
        /// Возвращает размер отрезка по оси Х
        /// он равен размеру кабины по оси Х
        /// </summary>
        public int SegmentXLen => sizes.CabinSizes.X;

        /// <summary>
        /// Возвращает количество отрезков по оси Y
        /// их количество равно 4 + кол-во перемычек в оси Х * 2 + кол-во перемычек по оси Z * 2
        /// </summary>
        public int SegmentYQuantity => 4 + sizes.CabinSizes.YsegmentsInXSurface * 2 + sizes.CabinSizes.YsegmentsInZsurface * 2;

        /// <summary>
        /// Длина отрезка Y
        /// расчитывает - Размер кабины Y - размер профиля Y * 2;
        /// </summary>
        public int SegmentYLen => sizes.CabinSizes.Y - sizes.ProfileSizes.Y * 2;

        /// <summary>
        /// Кол-во отрезков в Z плоскости.
        /// Расчитывается = 4 + кол-во перемычек кабины в оси X * 2;
        /// </summary>
        public int SegmentZQuantity => 4 + sizes.CabinSizes.YsegmentsInXSurface * 2;

        /// <summary>
        /// Длина отрезка Z 
        /// расчитывается как длина кабины минус размер профиля по оси Z на два
        /// </summary>
        public int SegmentZLen => sizes.CabinSizes.Z - sizes.ProfileSizes.Z * 2;

        public string DefaultString(string Axis, int quantity, int length) => $"Для оси {Axis} вам понадобится {quantity} отрезка, длиной {length} мм.";
        public string Xresult => DefaultString("X", SegmentXQuantity, SegmentXLen);
        public string Yresult => DefaultString("Y", SegmentYQuantity, SegmentYLen);
        public string Zresult => DefaultString("Z", SegmentZQuantity, SegmentZLen);

        public void CalcResult()
        {
            NotifyPropertyChanged("Xresult");
            NotifyPropertyChanged("Yresult");
            NotifyPropertyChanged("Zresult");
        }

    }

}

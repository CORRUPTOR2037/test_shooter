
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BaseRange<T> {

    public T Min;
    public T Max;

    public BaseRange(T min, T max) { this.Min = min; this.Max = max; }
}

[System.Serializable]
public class IntegerRange : BaseRange<int> {

    public IntegerRange(int min, int max) : base(min, max) { }

    public int RandomValue {
        get {
            return new Random().Next(Min, Max);
        }
    }
}

[System.Serializable]
public class FloatRange : BaseRange<float> {

    public FloatRange (float min, float max) : base(min, max) { }

    public float RandomValue {
        get {
            return (float) new Random().NextDouble() * (Max - Min) + Min;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModelBased<T>
{
    void SetupModel(T model);
}

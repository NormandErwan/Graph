#ifndef _LinearMap_
#define _LinearMap_

struct LinearMap
{
    float Coefficient;

    float DomainMin;

    float ImageMin;
};

float Get(LinearMap map, float value)
{
    return map.ImageMin + (value - map.DomainMin) * map.Coefficient;
}

float2 Get(LinearMap map, float2 value)
{
    return float2(Get(map, value.x), Get(map, value.y));
}

#endif // _LinearMap_

#include <iostream>
#include <string>
#include <sstream>
#include <noise/noise.h>
#include "noiseutils.h"

using namespace noise;

std::string GetName(int level, int i, int j)
{
	std::stringstream stream;
	stream <<"../../Resources/Images/l"<<level<<"c"<<i<<"_"<<j<<".bmp";

	return stream.str();
}

std::string GetName(int level)
{
	std::stringstream stream;
	stream <<"../../Resources/Images/l"<<level<<".bmp";

	return stream.str();
}

void Build(utils::NoiseMapBuilderPlane& builder, 
		   utils::RendererImage& renderer,
		   utils::WriterBMP& writer,
		   int level,
		   int i, int j)
{
	int step = 1 << level;

	if(i % step == 0 && j % step == 0)
	{
		std::string name = GetName(level, i, j);
		std::cout << name << std::endl;

		builder.SetBounds(i, i + step, j, j + step);
		builder.Build();

		renderer.Render();

		writer.SetDestFilename(name);
		writer.WriteDestFile();
	}
}

void Build(utils::NoiseMapBuilderPlane& builder, 
		   utils::RendererImage& renderer,
		   utils::WriterBMP& writer,
		   int level,
		   int maxLevel)
{
	int step = 1 << maxLevel;

	int size = ((1 << 8) << (maxLevel - level)) + 1;

	std::string name = GetName(level);
	std::cout << name << std::endl;

	builder.SetDestSize(size, size);
	builder.SetBounds(0, step, 0, step);
	builder.Build();

	renderer.Render();

	writer.SetDestFilename(name);
	writer.WriteDestFile();
}

void Build(utils::NoiseMapBuilderPlane& builder, 
		   utils::RendererImage& renderer,
		   utils::WriterBMP& writer,
		   int maxLevel)
{
	/*for(int i = 0; i < (1 << maxLevel); i++)
	{
		for(int j = 0; j < (1 << maxLevel); j++)
		{
			for(int level = 0; level <= maxLevel; level++)
			{
				Build(builder, renderer, writer, level, i, j);
			}
		}
	}*/


	for(int i = 0; i <= maxLevel; i++)
	{
		Build(builder, renderer, writer, i, maxLevel);
	}
}

int main (int argc, char** argv)
{
	module::RidgedMulti mountainTerrain;

	module::Billow baseFlatTerrain;
	baseFlatTerrain.SetFrequency (2.0);

	module::ScaleBias flatTerrain;
	flatTerrain.SetSourceModule (0, baseFlatTerrain);
	flatTerrain.SetScale (0.125);
	flatTerrain.SetBias (-0.75);

	module::Perlin terrainType;
	terrainType.SetFrequency (0.5);
	terrainType.SetPersistence (0.25);

	module::Select terrainSelector;
	terrainSelector.SetSourceModule (0, flatTerrain);
	terrainSelector.SetSourceModule (1, mountainTerrain);
	terrainSelector.SetControlModule (terrainType);
	terrainSelector.SetBounds (0.0, 1000.0);
	terrainSelector.SetEdgeFalloff (0.125);

	module::Turbulence finalTerrain;
	finalTerrain.SetSourceModule (0, terrainSelector);
	finalTerrain.SetFrequency (4.0);
	finalTerrain.SetPower (0.125);

	utils::NoiseMap heightMap;
	utils::NoiseMapBuilderPlane heightMapBuilder;
	heightMapBuilder.SetSourceModule (finalTerrain);
	heightMapBuilder.SetDestNoiseMap (heightMap);
	heightMapBuilder.SetDestSize (257, 257);

	utils::RendererImage renderer;
	utils::Image image;
	renderer.SetSourceNoiseMap (heightMap);
	renderer.SetDestImage (image);

	utils::WriterBMP writer;
	writer.SetSourceImage (image);

	Build(heightMapBuilder, renderer, writer, 4);

	return 0;
}
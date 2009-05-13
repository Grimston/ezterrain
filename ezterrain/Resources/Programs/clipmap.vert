//#version 130
#extension GL_EXT_gpu_shader4 : enable

uniform sampler2DArray noiseArray;

uniform float texScale;
uniform float texOffset;
uniform float heightScale;

uniform float level;

uniform vec3 eye;

const float morphRatio = 1.0 / 32.0;

in vec4 glVertex;

varying vec3 normal;
varying float bias;

vec4 calcTexCoord(vec4 vertex, float texScale)
{
	return (vertex) * texScale + texOffset;
}

vec4 calcTexCoord(vec4 vertex)
{
	return calcTexCoord(vertex, texScale);
}

float getBias(vec4 vertex)
{
	vec4 texCoord = calcTexCoord(vertex);

	float bias = texture2DArray(noiseArray, vec3(texCoord.st, level)).r;

	vec2 mor = texCoord.st - texOffset;
	
	mor = 2.0 * abs(mor) + vec2(morphRatio);
	
	bool morphx = mor.x > 1.0;
	bool morphy = mor.y > 1.0;
	
	float coarseBias;
	
	if(morphx || morphy)
	{
		coarseBias = texture2DArray(noiseArray, vec3(calcTexCoord(vertex, texScale / 2.0).st, level + 1.0)).r;
		
		float morph = ((morphx ? mor.x : mor.y) - 1.0) / morphRatio;
		
		return (1.0 - morph) * bias + morph * coarseBias;
	}
	else
	{
		return bias;
	}
}

float getHeight(float bias)
{
	return heightScale * bias;
}

float getHeight(vec4 vertex)
{
	return getHeight(getBias(vertex));
}

void main()
{	
	float vertexScale = pow(2.0, level);
	
	vec4 vertex = (glVertex  * vertexScale);
	
	bias = getBias(vertex);
	vertex.z = getHeight(bias);
	vertex.w = 1.0;
	
	vec4 komsu1 = vertex;
	//TODO: check bounds
	komsu1.x = komsu1.x + vertexScale;
	komsu1.z = getHeight(komsu1);
	vec3 delta1 = normalize((komsu1 - vertex).xyz);
	
	vec4 komsu2 = vertex;
	//TODO: check bounds
	komsu2.y = komsu2.y + vertexScale;
	komsu2.z = getHeight(komsu2);
	vec3 delta2 = normalize((komsu2 - vertex).xyz);
	
	vec4 komsu3 = vertex;
	//TODO: check bounds
	komsu3.x = komsu3.x - vertexScale;
	komsu3.z = getHeight(komsu3);
	vec3 delta3 = normalize((komsu3 - vertex).xyz);
	
	vec4 komsu4 = vertex;
	//TODO: check bounds
	komsu4.y = komsu4.y - vertexScale;
	komsu4.z = getHeight(komsu4);
	vec3 delta4 = normalize((komsu4 - vertex).xyz);

	vec3 normal1 = cross(delta1, delta2);
	vec3 normal2 = cross(delta2, delta3);
	vec3 normal3 = cross(delta3, delta4);
	vec3 normal4 = cross(delta4, delta1);
	normal = (normal1 + normal2 + normal3 + normal4) / 4.0;

	vertex.xy += eye.xy;
	
	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}

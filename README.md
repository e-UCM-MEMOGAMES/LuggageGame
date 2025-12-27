
## Cómo modificar el archivo de niveles

- Los ficheros a modificar están en ```Assets/Resources/LevelInfo```. A excepción del tutorial, los nombres de los niveles se identifican por su número y por el clima; por ejemplo, el archivo con los objetos del nivel 1 en el clima cálido sería ```Level1Warm.json```

- Cada fichero tiene 2 objetos esenciales: 

  - ```objectList```: tiene un array con los objetos a meter en la maleta según el género. Aquí solo es necesario rellenar los arrays correspondientes con las [IDs](#objetos) de los objetos.

  - ```spawnpoints```: tiene los puntos de almacenamiento de los objetos, siendo cada uno un objeto cuya clave es la [ID](#puntos-de-almacenaje) del punto, que contiene los arrays de los objetos a spawnear. Cada array se rellena con los objetos correspondientes, añadiendo objetos que tengan la propiedad ```id``` con la [ID](#objetos) del objeto y la propiedad ```position``` (```int```) con su posición en dicho punto de almacenamiento. Los puntos naranjas que se muestran en las imágenes son los puntos de almacenamiento colocados en la escena. 

 - En ambos casos, los géneros posibles son ```M``` (masculino), ```F``` (femenino) y ```N``` (neutro, que será válido para ambos géneros)
  
 - No es necesario definir todos los arrays para los géneros en ```objectList``` ni en cada punto de almacenaje, ni definir todos los puntos de almacenaje en ```spawnpoints```.

  <br>

  ![Nivel](./README_IMAGES/1.png "Nivel")
  ![Nivel](./README_IMAGES/2.png "Nivel")
  ![Nivel](./README_IMAGES/3.png "Nivel")
  ![Nivel](./README_IMAGES/4.png "Nivel")

<br>

Ejemplo:
```
{
	"objectList": {
		"M": [],
		"F": [],
		"N": [
			"GreenBlouse"
		]
	},
	"spawnpoints": {

		...,

		"Dresser": {
			"M": [],
			"F": [],
			"N": [
				{
					"id": "Cologne",
					"position": 0
				},
				{
					"id": "Comb",
					"position": 1
				}
			]
		},

		...,

	}
}
```
<br>

## Información necesaria para configurar el archivo de niveles

### Objetos
**NOTA**: Es recomendable configurar un máximo 12 de objetos en la lista de objetos a guardar en la maleta

| Objeto | ID (```string```) | Escena | Puntos de almacenaje |
| ------------- | ------------- | ------------- | ------------- |
| **Abrigo** | ```Coat``` | Dormitorio | Perchero |
| **Camisa hawaiana** | ```HawaiianShirt``` | Dormitorio | Cualquier cajón |
| **Blusa verde** | ```GreenBlouse``` | Dormitorio | Armario |
| **Camisa roja** | ```RedShirt``` | Dormitorio | Armario |
| **Camiseta amarilla** | ```YellowTShirt``` | Dormitorio | Armario |
| **Jersey** | ```Jersey``` | Dormitorio | Armario |
| **Camiseta verde** | ```GreenTShirt``` | Dormitorio | Cualquier cajón |
| **Camiseta de tirantes** | ```TankTop``` | Dormitorio | Cualquier cajón |
| **Pijama de tirantes** | ```StrappyPajamas``` | Dormitorio | Cualquier cajón |
| **Pijama de seda** | ```SilkPajamas``` | Dormitorio | Cualquier cajón |
| **Pijama de invierno** | ```WinterPajamas``` | Dormitorio | Armario |
| **Vestido** | ```Dress``` | Dormitorio | Armario |
| **Falda** | ```Skirt``` | Dormitorio | Cualquier cajón |
| **Bikini** | ```Bikini``` | Dormitorio | Cualquier cajón |
| **Bañador** | ```Swimsuit``` | Dormitorio | Cualquier cajón |
| **Bermudas** | ```Bermuda``` | Dormitorio | Cualquier cajón |
| **Pantalones largos** | ```Jeans``` | Dormitorio | Cualquier cajón |
| **Pantalones cortos** | ```Shorts``` | Dormitorio | Cualquier cajón |
| **Calcetines** | ```Socks``` | Dormitorio | Cualquier cajón |
| **Tacones** | ```Heels``` | Dormitorio | Zapatero |
| **Deportivas** | ```Sneakers``` | Dormitorio | Zapatero |
| **Zapatos de tela** | ```ClothShoes``` | Dormitorio | Zapatero |
| **Chanclas** | ```FlipFlops``` | Dormitorio | Zapatero |
| **Botas** | ```Boots``` | Dormitorio | Zapatero |
| **Gorra** | ```Cap``` | Dormitorio | Encima del zapatero/cómoda |
| **Bolso** | ```Handbag``` | Dormitorio | Perchero |
| **Riñonera** | ```FannyPack``` | Dormitorio | Encima del zapatero/cómoda |
| **Bufanda** | ```Scarf``` | Dormitorio | Cualquier cajón |
| **Gorro** | ```Beanie``` | Dormitorio | Cualquier cajón |
| **Cinturón** | ```Belt``` | Dormitorio | Encima del zapatero/cómoda |
| **Gafas de buceo** | ```DivingGoggles``` | Dormitorio | Encima del zapatero/cómoda |
| **Gafas de ski** | ```SkiGoggles``` | Dormitorio | Encima del zapatero/cómoda |
| **Reloj** | ```Watch``` | Dormitorio | Cualquier cajón |
| **Guía de viajes** | ```TravelGuide``` | Dormitorio | Cualquier cajón |
| **Libro** | ```Book``` | Dormitorio | Encima del zapatero/cómoda |
| **Pareo** | ```Sarong``` | Baño | Cualquier cajón |
| **Neceser** | ```ToiletryBag``` | Baño | Cualquier cajón |
| **Peine** | ```Comb``` | Baño | Armario del baño |
| **Pasta de dientes** | ```Toothpaste``` | Baño | Armario del baño |
| **Cepillo de dientes** | ```Toothbrush``` | Baño | Armario del baño |
| **Toalla** | ```Towel``` | Baño | Percha |
| **Colonia** | ```Cologne``` | Baño | Armario del baño |
| **Bote de medicinas** | ```Pills``` | Baño | Armario botiquín |

### Puntos de almacenaje 

| Puntos de almacenaje  | ID (```string```) | Escena | Huecos disponibles (```int```)|
| ------------- | ------------- | ------------- | ------------- |
| **Encima de la cómoda** | ```Dresser``` | Dormitorio | 2 (0-1)|
| **Cajón inferior de la cómoda** | ```Dresser_Drawer_bottom``` | Dormitorio | 6 (0-5)|
| **Cajón medio de la cómoda** | ```Dresser_Drawer_middle``` | Dormitorio | 6 (0-5)|
| **Cajón superior de la cómoda** | ```Dresser_Drawer_top``` | Dormitorio | 6 (0-5)|
| **Puerta de Armario** | ```Closet``` | Dormitorio | 2 (0-1)|
| **Cajón inferior del armario** | ```Closet_Drawer_bottom``` | Dormitorio | 6 (0-5)|
| **Cajón medio del armario** | ```Closet_Drawer_middle``` | Dormitorio | 6 (0-5)|
| **Cajón superior del armario** | ```Closet_Drawer_top``` | Dormitorio | 6 (0-5)|
| **Perchero** | ```CoatRack``` | Dormitorio | 2 (0-1)|
| **Encima del Zapatero** | ```ShoeRack_top``` | Dormitorio | 3 (0-2)|
| **Zapatero** | ```ShoeRack``` | Dormitorio | 6 (0-5)|
| **Percha** | ```Bathroom_Hanger``` | Baño | 2 (0-1)|
| **Encima del Lavabo** | ```Sink``` | Baño | 1 (0)|
| **Cajón superior del Lavabo** | ```Sink_Drawer_top``` | Baño | 6 (0-5)|
| **Cajón inferior del Lavabo** | ```Sink_Drawer_bottom``` | Baño | 6 (0-5)|
| **Armario del baño** | ```MirrorCabinet``` | Baño | 6 (0-5) |
| **Armario botiquín** | ```MedicineCabinet``` | Baño | 6 (0-5) |


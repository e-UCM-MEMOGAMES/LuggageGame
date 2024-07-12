
## Cómo modificar el archivo de niveles

- Los ficheros a modificar están en Assets/Resources/Lists. Por ejemplo, el nivel 1 con clima cálido sería Level1Warm.json
- En el fichero hay 4 grandes apartados: "objectList_M" , "objectList_F" y "objectList_N" son la lista de objetos a meter en la maleta para hombre, mujer y neutro, respectivamente. Aquí solo hay que poner el nombre del objeto.
- En "storagePoints" hay 16 puntos de almacenamiento. Simplemente hay que rellenar en el array de "objects" el nombre del objeto, su posición en ese sitio siendo un entero y el género M,F,N.
- Los puntos naranjas que se muestran en las imágenes son los puntos de almacenamiento colocados en la escena. <br>
 <br>
 
  ![Nivel](/README_IMAGES/1.png "Nivel")
  ![Nivel](/README_IMAGES/2.png "Nivel")
  ![Nivel](/README_IMAGES/3.png "Nivel")
  ![Nivel](/README_IMAGES/4.png "Nivel")

## Información necesaria para configurar el archivo de niveles

### Objetos
NOTA: Recomendable 12 objetos como máximo en la lista de objetos a guardar en la maleta

| Objeto ID | Escena | Puntos de almacenaje |
| ------------- | ------------- | ------------- |
| **Abrigo** | Dormitorio | Perchero |
| **Camisa hawaiana** | Dormitorio | Cualquier cajón |
| **Blusa verde** | Dormitorio | Puerta del armario |
| **Camisa roja** | Dormitorio | Puerta del armario |
| **Camiseta amarilla** | Dormitorio | Puerta del armario |
| **Jersey** | Dormitorio | Puerta del armario |
| **Camiseta verde** | Dormitorio | Cualquier cajón |
| **Camiseta de tirantes** | Dormitorio | Cualquier cajón |
| **Pijama de tirantes** | Dormitorio | Cualquier cajón |
| **Pijama de seda** | Dormitorio | Cualquier cajón |
| **Pijama de invierno** | Dormitorio | Puerta del armario |
| **Vestido** | Dormitorio | Puerta del armario |
| **Falda** | Dormitorio | Cualquier cajón |
| **Bikini** | Dormitorio | Cualquier cajón |
| **Banador** | Dormitorio | Cualquier cajón |
| **Bermudas** | Dormitorio | Cualquier cajón |
| **Pantalones largos** | Dormitorio | Cualquier cajón |
| **Pantalones cortos** | Dormitorio | Cualquier cajón |
| **Calcetines** | Dormitorio | Cualquier cajón |
| **Tacones** | Dormitorio | Zapatero |
| **Deportivas** | Dormitorio | Zapatero |
| **Zapatos de tela** | Dormitorio | Zapatero |
| **Chanclas** | Dormitorio | Zapatero |
| **Botas** | Dormitorio | Zapatero |
| **Gorra** | Dormitorio | Encima del zapatero/cómoda |
| **Bolso** | Dormitorio | Perchero |
| **Rinonera** | Dormitorio | Encima del zapatero/cómoda |
| **Bufanda** | Dormitorio | Cualquier cajón |
| **Gorro** | Dormitorio | Cualquier cajón |
| **Cinturon** | Dormitorio | Encima del zapatero/cómoda |
| **Gafas de buceo** | Dormitorio | Encima del zapatero/cómoda |
| **Gafas de ski** | Dormitorio | Encima del zapatero/cómoda |
| **Reloj** | Dormitorio | Cualquier cajón |
| **Guia de viajes** | Dormitorio | Cualquier cajón |
| **Libro** | Dormitorio | Encima del zapatero/cómoda |
| **Pareo** | Baño | Cualquier cajón |
| **Neceser** | Baño | Cualquier cajón |
| **Peine** | Baño | Cualquier cajón |
| **Pasta de dientes** | Baño | Cualquier cajón |
| **Cepillo de dientes** | Baño | Cualquier cajón |
| **Toalla** | Baño | Percha |
| **Colonia** | Baño | Encima del Armario de baño|
| **Bote de medicinas** | Baño | Armario botiquín |

### Puntos de almacenaje 

| Puntos de almacenaje  | ID | Escena | Huecos disponibles (INT)|
| ------------- | ------------- | ------------- | ------------- |
| **Encima de la cómoda** | Bedroom_DresserDrawer_above | Dormitorio | 2 (0-1)|
| **Cajón superior de la cómoda** | Bedroom_DresserDrawer_top | Dormitorio | 6 (0-5)|
| **Cajón medio de la cómoda** | Bedroom_DresserDrawer_middle | Dormitorio | 6 (0-5)|
| **Cajón inferior de la cómoda** | Bedroom_DresserDrawer_bottom | Dormitorio | 6 (0-5)|
| **Puerta de Armario** | Bedroom_ClosetDoor | Dormitorio | 2 (0-1)|
| **Cajón superior del armario** | Bedroom_ClosetDrawer_top | Dormitorio | 6 (0-5)|
| **Cajón medio del armario** | Bedroom_ClosetDrawer_middle | Dormitorio | 6 (0-5)|
| **Cajón inferior del armario** | Bedroom_ClosetDrawer_bottom | Dormitorio | 6 (0-5)|
| **Perchero** | Bedroom_CoatRack | Dormitorio | 2 (0-1)|
| **Encima del Zapatero** | Bedroom_ShoeRack_above | Dormitorio | 3 (0-2)|
| **Zapatero** | Bedroom_ShoeRack | Dormitorio | 6 (0-5)|
| **Percha** | Bathroom_hanger | Baño | 2 (0-1)|
| **Encima del Lavabo** | Bathroom_cabinet_above | Baño | 1 (0)|
| **Cajón superior del Lavabo** | Bathroom_cabinet_top | Baño | 6 (0-5)|
| **Cajón inferior del Lavabo** | Bathroom_cabinet_bottom | Baño | 6 (0-5)|

